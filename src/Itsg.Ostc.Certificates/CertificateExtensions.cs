using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Itsg.Ostc.Certificates
{
    /// <summary>
    /// Hilfsfunktionen für X509-Zertifikate
    /// </summary>
    public static class CertificateExtensions
    {
        private static readonly Oid _hashSha1 = new Oid("SHA1");
        private static readonly Oid _hashSha256 = new Oid("SHA256");

        /// <summary>
        /// Ist die Zertifikat-Signatur unter Verwendung SHA256 erstellt worden?
        /// </summary>
        /// <param name="cert">Das zu prüfende Zertifikat</param>
        /// <returns><code>true</code>, wenn die Signatur unter Verwendung von SHA256 erstellt wurde</returns>
        public static bool IsSha256(this X509Certificate2 cert)
        {
            return cert.GetSignatureAlgorithmForCert().Value == _hashSha256.Value;
        }

        /// <summary>
        /// Ermittlung der <see cref="Oid"/> des Zertifikat-Signatur-Verfahrens
        /// </summary>
        /// <param name="cert">Das zu prüfende Zertifikat</param>
        /// <returns>Die <see cref="Oid"/> des Zertifikat-Signatur-Verfahrens</returns>
        public static Oid GetSignatureAlgorithmForCert(this X509Certificate2 cert)
        {
            return GetSignatureAlgorithmForCert(cert, _hashSha1);
        }

        /// <summary>
        /// Ermittlung der <see cref="Oid"/> des Zertifikat-Signatur-Verfahrens
        /// </summary>
        /// <param name="cert">Das zu prüfende Zertifikat</param>
        /// <param name="defaultValue">Die Standard-<see cref="Oid"/>, wenn das Signatur-Verfahren nicht ermittelt werden konnte</param>
        /// <returns>Die <see cref="Oid"/> des Zertifikat-Signatur-Verfahrens</returns>
        public static Oid GetSignatureAlgorithmForCert(this X509Certificate2 cert, Oid defaultValue)
        {
            switch (cert.SignatureAlgorithm.FriendlyName)
            {
                case "sha256RSA":
                    return _hashSha256;
                case "sha1RSA":
                    return _hashSha1;
            }
            return defaultValue;
        }
    }
}
