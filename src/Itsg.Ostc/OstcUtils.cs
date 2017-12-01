using System;

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Itsg.Ostc
{
    /// <summary>
    /// Hilfsfunktionen
    /// </summary>
    internal static class OstcUtils
    {
        /// <summary>
        /// Berechnet den Hash des öffentlichen Schlüssels
        /// </summary>
        /// <param name="rsaPubKey">RSA-Schlüssel-Parameter</param>
        /// <returns>Der MD5-Hash</returns>
        public static byte[] CalculatePublicKeyHash(RsaKeyParameters rsaPubKey)
        {
            var rawPubKeyData = new RsaPublicKeyStructure(rsaPubKey.Modulus, rsaPubKey.Exponent).ToAsn1Object().GetDerEncoded();
            var pubKeyHash = DigestUtilities.CalculateDigest("MD5", rawPubKeyData);
            return pubKeyHash;
        }
    }
}
