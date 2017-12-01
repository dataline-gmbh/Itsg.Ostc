using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Itsg.Ostc.Certificates
{
    /// <summary>
    /// Die Empfänger-Zertifikate, die von der ITSG zur Verfügung gestellt werden
    /// </summary>
    public sealed class ReceiverCertificates
    {
        private readonly X509Certificate2[] _rootCertificates;
        private readonly X509Certificate2Collection _intermediateCertificates;

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="ReceiverCertificates"/> Klasse.
        /// </summary>
        /// <param name="certificates">Die Empfänger-Zertifikate</param>
        public ReceiverCertificates(IReadOnlyCollection<X509Certificate2> certificates)
        {
            var receiverCertificates = new Dictionary<string, X509Certificate2>();
            var rootCertificates = new List<X509Certificate2>();
            var immedCerts = new List<X509Certificate2>();
            foreach (var certificate in certificates)
            {
                var key = GetKey(certificate);
                if (key == null)
                {
                    if (certificate.SubjectName.Name == certificate.IssuerName.Name)
                    {
                        rootCertificates.Add(certificate);
                    }
                    else
                    {
                        immedCerts.Add(certificate);
                    }
                }
                else
                {
                    receiverCertificates.Add(key, certificate);
                }
            }

            _rootCertificates = rootCertificates.ToArray();
            var immedCertArray = immedCerts.ToArray();
            _intermediateCertificates = new X509Certificate2Collection(immedCertArray);
            IntermediateCertificates = immedCertArray;
            Certificates = receiverCertificates;
        }

        /// <summary>
        /// Holt die Root-Zertifikate
        /// </summary>
        public IReadOnlyCollection<X509Certificate2> RootCertificates => _rootCertificates;

        /// <summary>
        /// Holt die Zwischenzertifikate
        /// </summary>
        public IReadOnlyCollection<X509Certificate2> IntermediateCertificates { get; }

        /// <summary>
        /// Holt die Zuordnung von Betriebsnummern zu Empfänger-Zertifikaten
        /// </summary>
        public IReadOnlyDictionary<string, X509Certificate2> Certificates { get; }

        /// <summary>
        /// Ermittelt die Zertifikatskette anhand eines Zertifikats
        /// </summary>
        /// <param name="certificate">Das Zertifikat für das die Zertifikatskette ermittelt werden soll</param>
        /// <returns>Die Zertifikate, die - zusätzlich zum übergebenen <paramref name="certificate"/> - die
        /// Zertifikatskette bilden oder <code>null</code>, falls keine Zertifikatskette aufgebaut werden konnte.</returns>
        public CertificateChain GetCertificateChain(X509Certificate2 certificate)
        {
            var chain = new X509Chain();
#if !NET45
            using (chain)
#endif
            {
                chain.ChainPolicy.ExtraStore.AddRange(_rootCertificates);
                chain.ChainPolicy.ExtraStore.AddRange(_intermediateCertificates);
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                if (!chain.Build(certificate))
                    return null;
                if (chain.ChainStatus.Any(x => x.Status != X509ChainStatusFlags.NoError))
                    return null;
                var chainCerts = chain.ChainElements.Cast<X509ChainElement>().Skip(1).Select(x => x.Certificate).ToArray();
                var result = new CertificateChain(certificate, chainCerts);
                return result;
            }
        }

        /// <summary>
        /// Laden der Zertifikatskette aus dem Internet
        /// </summary>
        /// <param name="proxy">Der Web-Proxy, der zu verwenden ist</param>
        /// <returns>Die X509-Zertifikate, die von der Web-Seite geladen wurden</returns>
        public static async Task<ReceiverCertificates> Load(IWebProxy proxy = null)
        {
            var sourceUrl = "https://trustcenter-data.itsg.de/agv/annahme-sha256.agv";
            var certificates = await LoadReceiverCertificates(sourceUrl, proxy).ConfigureAwait(false);
            return new ReceiverCertificates(certificates);
        }

        /// <summary>
        /// Laden der Zertifikatskette aus dem Internet
        /// </summary>
        /// <param name="client">Der HttpClient, der zu verwenden ist</param>
        /// <returns>Die X509-Zertifikate, die von der Web-Seite geladen wurden</returns>
        public static async Task<ReceiverCertificates> Load(HttpClient client)
        {
            var sourceUrl = "https://trustcenter-data.itsg.de/agv/annahme-sha256.agv";
            var certificates = await LoadReceiverCertificates(client, sourceUrl).ConfigureAwait(false);
            return new ReceiverCertificates(certificates);
        }

        /// <summary>
        /// Laden der Zertifikatskette aus dem <see cref="TextReader"/>
        /// </summary>
        /// <param name="reader">Der <see cref="TextReader"/> aus dem die Zertifikate geladen werden</param>
        /// <returns>Die X509-Zertifikate, die von der Web-Seite geladen wurden</returns>
        public static ReceiverCertificates Load(TextReader reader)
        {
            return new ReceiverCertificates(Read(reader));
        }

        private static string GetKey(X509Certificate2 certificate)
        {
            var decodedName = certificate.SubjectName.Decode(X500DistinguishedNameFlags.UseNewLines);
            var bnItem = decodedName?.Split('\n')
                                    .Select(y => y.Trim('\r', ' ', '\t'))
                                    .LastOrDefault(x => x.StartsWith("OU=BN"));
            return bnItem?.Substring(5);
        }

        private static async Task<IReadOnlyCollection<X509Certificate2>> LoadReceiverCertificates(string url, IWebProxy proxy)
        {
            var clientHandler = new HttpClientHandler();
            if (proxy != null)
                clientHandler.Proxy = proxy;
            using (var client = new HttpClient(clientHandler))
            {
                return await LoadReceiverCertificates(client, url).ConfigureAwait(false);
            }
        }

        private static async Task<IReadOnlyCollection<X509Certificate2>> LoadReceiverCertificates(HttpClient client, string url)
        {
            var text = await client.GetStringAsync(url).ConfigureAwait(false);
            using (var reader = new StringReader(text))
            {
                return Read(reader);
            }
        }

        private static IReadOnlyCollection<X509Certificate2> Read(TextReader reader)
        {
            var certs = new List<X509Certificate2>();
            var buffer = new StringBuilder();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    var certData = Convert.FromBase64String(buffer.ToString());
                    var cert = new X509Certificate2(certData);
                    certs.Add(cert);
                    buffer.Clear();
                }
                else
                {
                    buffer.Append(line.Trim('\r'));
                }
            }

            if (buffer.Length != 0)
            {
                var certData = Convert.FromBase64String(buffer.ToString());
                var cert = new X509Certificate2(certData);
                certs.Add(cert);
            }

            return certs;
        }
    }
}
