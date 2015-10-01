using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CenterCLR.Sgml;

using Itsg.Ostc;

using JetBrains.Annotations;

using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

using RestSharp.Portable;
using RestSharp.Portable.Authenticators;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Client, über den auf die OSTC-Funktionen (V1) zugegriffen wird
    /// </summary>
    public class OstcClient
    {
        private static readonly CultureInfo CultureDe = new CultureInfo("de-DE");
        private static readonly Encoding Iso88591 = Encoding.GetEncoding("iso-8859-1");

        private static readonly List<OrderStatusParser.IOrderStatusParser> OrderStatusParsers = new List<OrderStatusParser.IOrderStatusParser>
        {
            new OrderStatusParser.OrderStatusParserV1(),
            new OrderStatusParser.OrderStatusParserV2(),
        };

        [NotNull]
        private IRestClient Client { get; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="client">IRestClient, der für die Kommunikation mit dem Server verwendet wird</param>
        /// <param name="credentials">Die Login-Informationen, damit der Client auf den OSTC-Server zugreifen darf</param>
        public OstcClient([NotNull] IRestClient client, [CanBeNull] ICredentials credentials)
        {
            Client = client;
            if (credentials != null)
            {
                Client.Authenticator = new HttpDigestAuthenticator(AuthHeader.Www);
                Client.Credentials = credentials;
            }
            Client.ClearHandlers();
        }

        private static XDocument DecodeResponse(Stream stream)
        {
            var responseXml = SgmlReader.Parse(stream);
            return responseXml;
        }

        private static Uri GetResponseFileUrl(XDocument htmlDocument)
        {
            var xmlFileElement = htmlDocument
                .Elements("html")
                .Elements("body")
                .Elements("blockquote")
                .Elements("form")
                .Elements("table")
                .Elements("tr")
                .Elements("td")
                .Elements("name")
                .FirstOrDefault();
            if (xmlFileElement == null)
                return null;
            var resultUrl = new Uri(xmlFileElement.Value.Trim());
            return resultUrl;
        }

        /// <summary>
        /// Antrag versenden
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <param name="p10Data">PKCS#10-Daten</param>
        /// <returns>Das Ergebnis der Antragstellung</returns>
        public Task<OstcApplicationResult> SendApplicationAsync([NotNull] OstcAntrag application, [NotNull] Pkcs10Data p10Data)
        {
            return SendApplicationAsync(application, p10Data, null, null, null);
        }

        /// <summary>
        /// Antrag versenden
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <param name="p10Data">PKCS#10-Daten</param>
        /// <param name="validator">The validator for the OSTC certificate request document</param>
        /// <returns>Das Ergebnis der Antragstellung</returns>
        public Task<OstcApplicationResult> SendApplicationAsync([NotNull] OstcAntrag application, [NotNull] Pkcs10Data p10Data, IValidator validator)
        {
            return SendApplicationAsync(application, p10Data, null, null, validator);
        }

        /// <summary>
        /// Antrag versenden
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <param name="p10Data">PKCS#10-Daten</param>
        /// <param name="certStore">Zertifikat-Speicher für die Bildung der Zertifikatskette und die Abfrage des Empfänger-Zertifikats</param>
        /// <param name="pfx">Zertifikat für die Verschlüsselung - wenn nicht gesetzt, dann wird ein Erstantrag erstellt</param>
        /// <returns>Das Ergebnis der Antragstellung</returns>
        public Task<OstcApplicationResult> SendApplicationAsync([NotNull] OstcAntrag application, [NotNull] Pkcs10Data p10Data, [CanBeNull] IOstcCertificateStore certStore, [CanBeNull] Pkcs12Store pfx)
        {
            return SendApplicationAsync(application, p10Data, certStore, pfx, null);
        }

        /// <summary>
        /// Antrag versenden
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <param name="p10Data">PKCS#10-Daten</param>
        /// <param name="certStore">Zertifikat-Speicher für die Bildung der Zertifikatskette und die Abfrage des Empfänger-Zertifikats</param>
        /// <param name="pfx">Zertifikat für die Verschlüsselung - wenn nicht gesetzt, dann wird ein Erstantrag erstellt</param>
        /// <param name="validator">The validator for the OSTC certificate request document</param>
        /// <returns>Das Ergebnis der Antragstellung</returns>
        public async Task<OstcApplicationResult> SendApplicationAsync([NotNull] OstcAntrag application, [NotNull] Pkcs10Data p10Data, [CanBeNull] IOstcCertificateStore certStore, [CanBeNull] Pkcs12Store pfx, [CanBeNull] IValidator validator)
        {
            var senderId = SenderId.FromBnrOrIk(application.Antragsteller.IK_BN);

            var applicationData = OstcUtils.Serialize(application, Iso88591);
            string mimeType;
            if (pfx == null)
            {
                mimeType = "text/xml";
            }
            else
            {
                var alias = pfx.Aliases.Cast<string>().FirstOrDefault(pfx.IsKeyEntry);
                if (alias != null)
                {
                    var certEntry = pfx.GetCertificate(alias);
                    var keyEntry = pfx.GetKey(alias);
                    var certificate = certEntry.Certificate;
                    var key = keyEntry.Key;
                    var certChain = certStore.GetChain(certificate).ToList();
                    System.Diagnostics.Debug.Assert(certChain[0].SubjectDN.Equivalent(certificate.SubjectDN));
                    certChain.RemoveAt(0);
                    applicationData = OstcUtils.SignData(applicationData, key, certificate, certChain);
                    var receiverCert = certStore.GetCertificate(senderId.CommunicationServerReceiver);
                    applicationData = OstcUtils.EncryptData(applicationData, receiverCert);
                    mimeType = "application/octet-stream";
                }
                else
                {
                    mimeType = "text/xml";
                }
            }

            var date = DateTime.ParseExact(application.Antragsinfo.Datum, "dd.MM.yyyy", CultureDe);
            var p10FileName = $"{senderId.Id}.p10";
            var xmlFileName = $"{senderId}_{date:ddMMyyyy}.xml";

            var requestSendApp = new RestRequest(Network.Requests.Upload);
            requestSendApp.AddParameter("MAX_FILE_SIZE_XML", "4000");
            requestSendApp.AddFile("xml_Datei", applicationData, xmlFileName, mimeType);
            requestSendApp.AddParameter("MAX_FILE_SIZE_P10", "4000");
            requestSendApp.AddFile("p10_Datei", p10Data.CertRequestDer, p10FileName);

            var responseSendApp = await Client.Execute(requestSendApp);
            var responseHtml = DecodeResponse(new MemoryStream(responseSendApp.RawBytes));
            var responseFileUrl = GetResponseFileUrl(responseHtml);
            if (responseFileUrl == null)
                throw new OstcException("Von der ITSG wurde kein Pfad zu einer Rückmeldungs-Datei geliefert.");

            var downloadRequest = new RestRequest(responseFileUrl, Method.GET);
            var downloadResponse = await Client.Execute(downloadRequest);
            var resultData = downloadResponse.RawBytes;

            var resultXml = XDocument.Load(new MemoryStream(resultData));

            var trustCenterNode = resultXml
                .Elements("OSTCAntrag")
                .Elements("Trustcenter")
                .FirstOrDefault();
            if (trustCenterNode == null)
                throw new InvalidOperationException("Die von der ITSG zurückgelieferte Antwort lag in einem unbekannten Format vor.");
            var returnCodeNode = trustCenterNode
                .Elements("Returncode")
                .FirstOrDefault();
            if (returnCodeNode == null)
                throw new InvalidOperationException("Die von der ITSG zurückgelieferte Antwort lag in einem unbekannten Format vor.");
            var returnCode = Convert.ToInt32(returnCodeNode.Value.Trim(), 10);
            var errorCodeNode = trustCenterNode.Elements("Fehlercode").FirstOrDefault();
            if (errorCodeNode == null)
                throw new InvalidOperationException("Die von der ITSG zurückgelieferte Antwort lag in einem unbekannten Format vor.");
            var errorCodes = errorCodeNode.Value.Trim()
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x.Trim(), 10))
                .ToList();

            var inputNumberNode = trustCenterNode.Elements("Eingangsnummer").FirstOrDefault();
            if (inputNumberNode == null)
                throw new InvalidOperationException("Die von der ITSG zurückgelieferte Antwort lag in einem unbekannten Format vor.");
            var inputNumber = inputNumberNode.Value.Trim();
            var result = new OstcApplicationResult()
            {
                OrderId = inputNumber,
                ReturnCode = returnCode,
                ErrorCodes = errorCodes,
            };

            return result;
        }

        /// <summary>
        /// Antragsstatus erfragen
        /// </summary>
        /// <param name="sender">Absender-ID</param>
        /// <param name="orderId">Auftrags-ID</param>
        /// <returns>Status des Auftrags</returns>
        /// <remarks>Die Auftrags-ID wird von der Funktion <see cref="SendApplicationAsync(OstcAntrag,Pkcs10Data,IOstcCertificateStore,Pkcs12Store)"/> zurückgeliefert</remarks>
        public async Task<OstcOrderResult> QueryOrderStatusAsync(SenderId sender, string orderId)
        {
            var request = new RestRequest(Network.Requests.Order);
            request.AddParameter("ik_bn", sender.Id);
            request.AddParameter("id", orderId);

            var response = await Client.Execute(request);
            var responseHtml = DecodeResponse(new MemoryStream(response.RawBytes));

            OstcOrderResult result =
                (from parser in OrderStatusParsers
                 where parser.IsApplicable(responseHtml)
                 select parser.ExtractResult(response.ResponseUri, responseHtml))
                    .FirstOrDefault()
                ?? new OstcOrderResult()
                {
                    Status = OstcOrderStatus.NotFound
                };

            return result;
        }

        /// <summary>
        /// Zertifikat von einer URL herunterladen
        /// </summary>
        /// <param name="downloadUrl">Die URL von der das Zertifikat heruntergeladen werden soll</param>
        /// <returns>Zertifikat (mit zusätzlichen Zertifikaten, damit eine vollständige Zertifikat-Kette erstellt werden kann)</returns>
        /// <remarks>
        /// Die URL wird von der Funktion <see cref="QueryOrderStatusAsync"/> zurückgeliefert, wenn der <see cref="OstcOrderStatus"/>
        /// den Wert <see cref="OstcOrderStatus.Successful"/> hat.
        /// </remarks>
        public async Task<IReadOnlyList<X509Certificate>> DownloadCertificateAsync(Uri downloadUrl)
        {
            var request = new RestRequest(downloadUrl);
            var response = await Client.Execute(request);
            var certData = response.RawBytes;
            
            var parser = new X509CertificateParser();
            var certs = parser.ReadCertificates(certData).Cast<X509Certificate>().ToList();

            return certs;
        }

        /// <summary>
        /// Zertifikat-Liste vom OSTC-Server laden
        /// </summary>
        /// <param name="listType">Art der Zertifikat-Liste</param>
        /// <param name="certType">Art des Zertifikats (SHA1 oder SHA256)</param>
        /// <param name="preferFtp">FTP-Server-Download bevorzugen?</param>
        /// <returns>Liste der Zertifikate</returns>
        public async Task<IReadOnlyList<X509Certificate>> DownloadCertificateListAsync(OstcCertificateListType listType, OstcCertificateType certType, bool preferFtp = true)
        {
            var linkContents = new List<string>();
            switch (listType)
            {
                case OstcCertificateListType.Receiver:
                    if ((certType & OstcCertificateType.Sha1) == OstcCertificateType.Sha1)
                        linkContents.Add("annahme-pkcs");
                    if ((certType & OstcCertificateType.Sha256) == OstcCertificateType.Sha256)
                        linkContents.Add("annahme-sha256");
                    break;
                case OstcCertificateListType.Complete:
                    if (certType == (OstcCertificateType.Sha1 | OstcCertificateType.Sha256))
                    {
                        linkContents.Add("gesamt-pkcs");
                    }
                    else
                    {
                        if ((certType & OstcCertificateType.Sha1) == OstcCertificateType.Sha1)
                            linkContents.Add("gesamt-sha1");
                        if ((certType & OstcCertificateType.Sha256) == OstcCertificateType.Sha256)
                            linkContents.Add("gesamt-sha256");
                    }
                    break;
            }

            var data = await DownloadAsync(
                linkContents,
                downloadUrl =>
                {
                    var path = downloadUrl.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                    return path.EndsWith(".agv", StringComparison.OrdinalIgnoreCase);
                },
                preferFtp);

            var allCerts = new List<X509Certificate>();
            allCerts.AddRange(OstcUtils.ReadCertificates(new MemoryStream(data)));

            return allCerts;
        }


        /// <summary>
        /// Zertifikatsperrliste vom OSTC-Server laden
        /// </summary>
        /// <param name="certType">Art des Zertifikats (SHA1 oder SHA256)</param>
        /// <param name="preferFtp">FTP-Server-Download bevorzugen?</param>
        /// <returns>Zertifikatsperrliste</returns>
        public async Task<X509Crl> DownloadCrlAsync(OstcCertificateType certType, bool preferFtp = true)
        {
            var linkContents = new List<string>();
            if ((certType & OstcCertificateType.Sha1) == OstcCertificateType.Sha1)
                linkContents.Add("sperrliste-ag");
            if ((certType & OstcCertificateType.Sha256) == OstcCertificateType.Sha256)
                linkContents.Add("sperrliste-ag-sha256");

            var data = await DownloadAsync(
                linkContents,
                downloadUrl =>
                {
                    var path = downloadUrl.GetComponents(UriComponents.Path, UriFormat.Unescaped);
                    return path.EndsWith(".crl", StringComparison.OrdinalIgnoreCase);
                },
                preferFtp);

            var crlParser = new X509CrlParser();
            var crl = crlParser.ReadCrl(data);

            return crl;
        }

        /// <summary>
        /// Zertifikat-Liste vom OSTC-Server laden
        /// </summary>
        /// <param name="linkContents">Die Texte die im Link vorkommen müssen</param>
        /// <param name="downloadUrlsFilter">Filterung der Download-URLs</param>
        /// <param name="preferFtp">FTP-Server-Download bevorzugen?</param>
        /// <returns>Die geladenen Daten</returns>
        private async Task<byte[]> DownloadAsync(IEnumerable<string> linkContents, Func<Uri, bool> downloadUrlsFilter, bool preferFtp = true)
        {
            var url = new Uri("http://www.itsg.de/tc_keys_arbeitgeberverfahren.ITSG");
            var webPageRequest = new RestRequest(url);
            var webPageResponse = await Client.Execute(webPageRequest);
            var webPage = webPageResponse.RawBytes;
            var responseXml = SgmlReader.Parse(new MemoryStream(webPage));

            var downloadUrls = new List<Uri>();

            var ns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            foreach (var linkContent in linkContents)
            {
                var httpDownloadUrl = responseXml
                    .Descendants(ns + "a")
                    .Where(x => x.Value.Trim() == "HTTP-Server")
                    .Where(x => x.Attributes("href").Any(y => y.Value.Contains(linkContent)))
                    .Select(x => x.Attribute("href"))
                    .FirstOrDefault();
                var ftpDownloadUrl = responseXml
                    .Descendants(ns + "a")
                    .Where(x => x.Value.Trim() == "FTP-Server")
                    .Where(x => x.Attributes("href").Any(y => y.Value.Contains(linkContent)))
                    .Select(x => x.Attribute("href"))
                    .FirstOrDefault();
                if (preferFtp)
                {
                    if (ftpDownloadUrl != null)
                        downloadUrls.Add(new Uri(url, ftpDownloadUrl.Value));
                    if (httpDownloadUrl != null)
                        downloadUrls.Add(new Uri(url, httpDownloadUrl.Value));
                }
                else
                {
                    if (httpDownloadUrl != null)
                        downloadUrls.Add(new Uri(url, httpDownloadUrl.Value));
                    if (ftpDownloadUrl != null)
                        downloadUrls.Add(new Uri(url, ftpDownloadUrl.Value));
                }
            }

            var usefulDownloadUrls = downloadUrls.Where(downloadUrlsFilter).ToList();

            var exceptions = new List<Exception>();
            foreach (var usefulDownloadUrl in usefulDownloadUrls)
            {
                try
                {
                    if (string.Equals(usefulDownloadUrl.Scheme, "ftp", StringComparison.OrdinalIgnoreCase))
                    {
                        // Special handling for FTP access
                        var downloadRequest = WebRequest.Create(usefulDownloadUrl);
                        var downloadResponse = await Task.Factory.FromAsync<WebResponse>(downloadRequest.BeginGetResponse, downloadRequest.EndGetResponse, null);
                        using (var temp = new MemoryStream())
                        {
                            var responseStream = downloadResponse.GetResponseStream();
                            var buffer = new byte[100 * 1024];
                            int readCount;
                            //var totalReadCount = 0;
                            while ((readCount = responseStream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                temp.Write(buffer, 0, readCount);
                                //totalReadCount += readCount;
                                //Debug.WriteLine(totalReadCount);
                            }
                            return temp.ToArray();
                        }
                    }
                    else
                    {
                        var downloadRequest = new RestRequest(usefulDownloadUrl);
                        var downloadResponse = await Client.Execute(downloadRequest);
                        return downloadResponse.RawBytes;
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count != 0)
            {
                // Unable to download certificates
                throw new AggregateException(exceptions);
            }

            return null;
        }
    }
}
