using System;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Art des Zertifikats
    /// </summary>
    [Flags]
    public enum OstcCertificateType
    {
        /// <summary>
        /// SHA1-Zertifikat
        /// </summary>
        [Obsolete("Es wird nur noch SHA256 unterstützt")]
        Sha1 = 1,

        /// <summary>
        /// SHA256-Zertifikat
        /// </summary>
        Sha256 = 2,
    }
}
