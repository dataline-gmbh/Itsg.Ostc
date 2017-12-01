using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Hilfsfunktionen
    /// </summary>
    internal static class OstcUtils
    {
        /// <summary>
        /// Signiert die Daten mit dem angegebenen Absender-Zertifikat
        /// </summary>
        /// <param name="data">Die zu signierenden Daten</param>
        /// <param name="privateKey">Der private Schlüssel mit dem signiert werden soll (passend zum Zeritifikat <paramref name="cert"/>)</param>
        /// <param name="cert">Das Absender-Zertifikat</param>
        /// <param name="certs">Die Zertifikate, die zusätzlich im Ergebnis gespeichert werden sollen (z.B. für eine Zertifkatskette)</param>
        /// <returns>Die signierten Daten</returns>
        public static byte[] SignData(byte[] data, AsymmetricKeyParameter privateKey, X509Certificate cert, IEnumerable<X509Certificate> certs = null)
        {
            var gen = new CmsSignedDataGenerator();
            var allCerts = new List<X509Certificate>();
            if (certs != null)
                allCerts.AddRange(certs);
            var storeParams = new X509CollectionStoreParameters(allCerts);
            var certStore = X509StoreFactory.Create("Certificate/Collection", storeParams);
            gen.AddCertificates(certStore);
            gen.AddSigner(privateKey, cert, NistObjectIdentifiers.IdSha256.Id);
            var message = new CmsProcessableByteArray(data);
            var signedData = gen.Generate(message, true);
            return signedData.GetEncoded();
        }

        /// <summary>
        /// Verschlüsselt die Daten mit dem angegebenen Empfänger-Zertifikat
        /// </summary>
        /// <param name="data">Die zu verschlüsselnden Daten</param>
        /// <param name="cert">Das Empfänger-Zertifikat</param>
        /// <returns>Die verschlüsselten Daten</returns>
        public static byte[] EncryptData(byte[] data, X509Certificate cert)
        {
            var gen = new CmsEnvelopedDataGenerator();
            gen.AddKeyTransRecipient(cert);
            var message = new CmsProcessableByteArray(data);
            var envelopedData = gen.Generate(message, PkcsObjectIdentifiers.DesEde3Cbc.Id);
            var encryptedData = envelopedData.GetEncoded();
            return encryptedData;
        }

        /// <summary>
        /// Liest die Zertifikate aus einer Zertifikats-Datei der ITSG aus
        /// </summary>
        /// <param name="stream">Datenstrom in dem die ITSG-Zertifikate enthalten sind</param>
        /// <returns>Liste aller Zertifikate aus dem Datenstrom</returns>
        public static IReadOnlyList<X509Certificate> ReadCertificates(Stream stream)
        {
            var result = new List<X509Certificate>();
            var parser = new X509CertificateParser();
            var cert = new StringBuilder();
            var reader = new StreamReader(stream);
            var lines = reader.ReadToEnd().Split('\n').Select(x => x.TrimEnd(' ', '\t', '\r')).ToList();
            lines.Add(string.Empty);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (cert.Length != 0)
                    {
                        cert.AppendLine("-----END CERTIFICATE-----");

                        var byteArray = Encoding.UTF8.GetBytes(cert.ToString());
                        var importedCert = parser.ReadCertificate(byteArray);
                        result.Add(importedCert);

                        cert = new StringBuilder();
                    }
                }
                else
                {
                    if (cert.Length == 0)
                        cert.AppendLine("-----BEGIN CERTIFICATE-----");
                    cert.AppendLine(line);
                }
            }
            return result;
        }

        /// <summary>
        /// Serialisiert ein Objekt
        /// </summary>
        /// <param name="value">Zu serialisierendes Objekt</param>
        /// <param name="encoding">Zeichensatz, der für die Erstellung der XML-Datei verwendet wird</param>
        /// <returns>Serialisiertes Objekt</returns>
        public static byte[] Serialize<T>(T value, Encoding encoding)
        {
            var serializer = new XmlSerializer(typeof(T));
            var output = new MemoryStream();
            var writerSettings = new System.Xml.XmlWriterSettings()
            {
                Encoding = encoding,
                Indent = true,
                NamespaceHandling = System.Xml.NamespaceHandling.OmitDuplicates,
                NewLineChars = "\n",
            };
            using (var writer = System.Xml.XmlWriter.Create(output, writerSettings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                serializer.Serialize(writer, value, namespaces);
            }
            return output.ToArray();
        }
    }
}
