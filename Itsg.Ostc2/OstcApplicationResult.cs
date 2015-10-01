using Org.BouncyCastle.Crypto.Parameters;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Ergebnis der Antragstellung
    /// </summary>
    public class OstcApplicationResult
    {
        /// <summary>
        /// ID des Auftrags
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// Hash des öffentlichen Schlüssels
        /// </summary>
        public byte[] Hash { get; set; }
        /// <summary>
        /// PKCS#10-Datei
        /// </summary>
        public byte[] Pkcs10 { get; set; }
        /// <summary>
        /// RSA-Schlüssel
        /// </summary>
        public RsaPrivateCrtKeyParameters RSA { get; set; }
    }
}
