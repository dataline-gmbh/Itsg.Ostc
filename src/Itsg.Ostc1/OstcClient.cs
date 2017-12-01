using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using CenterCLR.Sgml;

using Itsg.Ostc;

using JetBrains.Annotations;

using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

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
        private readonly HttpClient _client;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="client">HttpClient, der für die Kommunikation mit dem Server verwendet wird</param>
        public OstcClient([NotNull] HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="messageHandler">HttpClientHandler, der für die Kommunikation mit dem Server verwendet wird</param>
        /// <param name="baseUrl">Die Basis-Adresse des zu verwendenden Servers</param>
        public OstcClient([NotNull] HttpMessageHandler messageHandler, [NotNull] Uri baseUrl)
            : this(new HttpClient(messageHandler) { BaseAddress = baseUrl, })
        {
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="baseUrl">Die Basis-Adresse des zu verwendenden Servers</param>
        /// <param name="credentials">Die Login-Informationen, damit der Client auf den OSTC-Server zugreifen darf</param>
        public OstcClient([NotNull] Uri baseUrl, [CanBeNull] ICredentials credentials = null)
        {
            var handler = new HttpClientHandler();
            if (credentials != null)
                handler.Credentials = credentials;
            _client = new HttpClient(handler);
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
                    if (certStore == null)
                        throw new ArgumentNullException(nameof(certStore));
                    var certChain = certStore.GetChain(certificate).ToList();
                    Debug.Assert(certChain[0].SubjectDN.Equivalent(certificate.SubjectDN));
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

            var reqSendAppContent = new MultipartFormDataContent
            {
                { new StringContent("4000"), "MAX_FILE_SIZE_XML" },
                {
                    new ByteArrayContent(applicationData)
                    {
                        Headers = {
                            ContentType = MediaTypeHeaderValue.Parse(mimeType),
                        }
                    },
                    "xml_Datei",
                    xmlFileName
                },
                { new StringContent("4000"), "MAX_FILE_SIZE_P10" },
                {
                    new ByteArrayContent(p10Data.CertRequestDer)
                    {
                        Headers = {
                            ContentType = MediaTypeHeaderValue.Parse("application/octet-stream"),
                        }
                    },
                    "p10_Datei",
                    p10FileName
                }
            };

            var requestSendApp = new HttpRequestMessage(HttpMethod.Post, Network.Requests.Upload)
            {
                Content = reqSendAppContent,
            };

            var responseSendApp = await _client.SendAsync(requestSendApp);
            responseSendApp.EnsureSuccessStatusCode();

            var responseHtml = DecodeResponse(await responseSendApp.Content.ReadAsStreamAsync());
            var responseFileUrl = GetResponseFileUrl(responseHtml);
            if (responseFileUrl == null)
                throw new OstcException("Von der ITSG wurde kein Pfad zu einer Rückmeldungs-Datei geliefert.");

            var downloadResponse = await _client.GetAsync(responseFileUrl);
            downloadResponse.EnsureSuccessStatusCode();
            var resultData = await downloadResponse.Content.ReadAsByteArrayAsync();

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
            var request = new HttpRequestMessage(HttpMethod.Get, $"{Network.Requests.Order}?ik_bn={sender.Id}&id={orderId}");

            var response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseHtml = DecodeResponse(await response.Content.ReadAsStreamAsync());
            var responseUri = response.Headers.Location ?? request.RequestUri;

            OstcOrderResult result =
                (from parser in OrderStatusParsers
                 where parser.IsApplicable(responseHtml)
                 select parser.ExtractResult(responseUri, responseHtml))
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
            var response = await _client.GetAsync(downloadUrl);
            response.EnsureSuccessStatusCode();
            var certData = await response.Content.ReadAsByteArrayAsync();
            
            var parser = new X509CertificateParser();
            var certs = parser.ReadCertificates(certData).Cast<X509Certificate>().ToList();

            return certs;
        }

        /// <summary>
        /// Zertifikat-Liste vom OSTC-Server laden
        /// </summary>
        /// <param name="listType">Art der Zertifikat-Liste</param>
        /// <param name="certType">Art des Zertifikats (SHA1 oder SHA256)</param>
        /// <returns>Liste der Zertifikate</returns>
        public async Task<IReadOnlyList<X509Certificate>> DownloadCertificateListAsync(OstcCertificateListType listType, OstcCertificateType certType)
        {
            Uri downloadUrl = null;
            switch (listType)
            {
                case OstcCertificateListType.Receiver:
                    downloadUrl = new Uri("https://trustcenter-data.itsg.de/agv/annahme-sha256.agv");
                    break;
                case OstcCertificateListType.Complete:
#pragma warning disable CS0618 // Typ oder Element ist veraltet
                    if (certType == (OstcCertificateType.Sha1 | OstcCertificateType.Sha256))
#pragma warning restore CS0618 // Typ oder Element ist veraltet
                    {
                        // Mehr als nur SHA256
                        downloadUrl = new Uri("https://trustcenter-data.itsg.de/agv/gesamt-pkcs.agv");
                    }
                    else
                    {
                        downloadUrl = new Uri("https://trustcenter-data.itsg.de/agv/gesamt-sha256.agv");
                    }
                    break;
            }

            Debug.Assert(downloadUrl != null);

            var data = await DownloadAsync(downloadUrl);

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
            Uri downloadUrl = new Uri("https://trustcenter-data.itsg.de/agv/sperrliste-ag-sha256.crl");

            Debug.Assert(downloadUrl != null);

            var data = await DownloadAsync(downloadUrl);

            var crlParser = new X509CrlParser();
            var crl = crlParser.ReadCrl(data);

            return crl;
        }

        /// <summary>
        /// Zertifikat-Liste vom OSTC-Server laden
        /// </summary>
        /// <param name="downloadUrl">Die URL von der die Daten geladen werden sollen</param>
        /// <returns>Die geladenen Daten</returns>
        private async Task<byte[]> DownloadAsync(Uri downloadUrl)
        {
            var downloadResponse = await _client.GetAsync(downloadUrl);
            downloadResponse.EnsureSuccessStatusCode();
            return await downloadResponse.Content.ReadAsByteArrayAsync();
        }
    }
}
