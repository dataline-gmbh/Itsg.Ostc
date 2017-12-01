using System;

namespace Itsg.Ostc1
{
    /// <summary>
    /// OSTC-Zertfikats-Listen
    /// </summary>
    public enum OstcCertificateListType
    {
        /// <summary>
        /// Liste der Empfänger-Zertifikate
        /// </summary>
        Receiver,
        /// <summary>
        /// Vollständige Liste der Zertifikate (auch abgelaufen?)
        /// </summary>
        Complete,
    }
}
