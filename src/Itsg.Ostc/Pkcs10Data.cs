using System;
using System.Linq;

namespace Itsg.Ostc
{
    /// <summary>
    /// Die für die Erstellung der PKCS#10-Datei notwendigen Daten
    /// </summary>
    public class Pkcs10Data
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="certRequestDer">PKCS#10-Datei im DER-Format (binär, ASN.1-Kodiert)</param>
        /// <param name="certRequestPem">PKCS#10-Datei im PEM-Format</param>
        /// <param name="privateKeyPem">Privater Schlüssel im PEM-Format</param>
        /// <param name="publicKeyHash">Hash des öffentlichen Schlüssels</param>
        public Pkcs10Data(byte[] certRequestDer, string certRequestPem, string privateKeyPem, byte[] publicKeyHash)
        {
            CertRequestDer = certRequestDer;
            CertRequestPem = certRequestPem;
            PrivateKeyPem = privateKeyPem;
            PublicKeyHashRaw = publicKeyHash;
            PublicKeyHash = string.Join(string.Empty, publicKeyHash.Select(x => x.ToString("x2")));
        }

        /// <summary>
        /// Die P10-Datei im DER-Format
        /// </summary>
        public byte[] CertRequestDer { get; private set; }

        /// <summary>
        /// Die P10-Datei im PEM-Format
        /// </summary>
        public string CertRequestPem { get; private set; }

        /// <summary>
        /// Der private RSA-Schlüssel im PEM-Format (Paßwort-geschützt)
        /// </summary>
        public string PrivateKeyPem { get; private set; }

        /// <summary>
        /// Der MD5-Hash des öffentlichen Schlüssels
        /// </summary>
        public string PublicKeyHash { get; private set; }

        /// <summary>
        /// Der MD5-Hash des öffentlichen Schlüssels
        /// </summary>
        public byte[] PublicKeyHashRaw { get; private set; }
    }
}
