using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Org.BouncyCastle.X509;

namespace Itsg.Ostc
{
    /// <summary>
    /// Zertifikats-Speicher für die OSTC-Zertifikate
    /// </summary>
    public interface IOstcCertificateStore
    {
        /// <summary>
        /// Das aktuelle Zertifikat für den angegebenen Empfänger
        /// </summary>
        /// <param name="receiverId">Empfänger-ID</param>
        /// <returns>Das aktuelle Zertifikat für den Empfänger oder null</returns>
        [CanBeNull]
        X509Certificate GetCertificate([NotNull] ReceiverId receiverId);

        /// <summary>
        /// Liefert die vollständige Zertifikatskette für das angegebene Zertifikat
        /// </summary>
        /// <param name="certificate">Das Zertifikat für das eine Zertifikatskette bis zum Root-Zertifikat der ITSG aufgebaut werden soll</param>
        /// <returns>Liste mit allen Zertifikaten, die für die Zertifikatskette notwendig sind</returns>
        /// <remarks>Das übergebene <paramref name="certificate"/> kann sowohl ein Absender- als auch ein Empfänger-Zeritifikat sein.</remarks>
        [NotNull]
        IReadOnlyCollection<X509Certificate> GetChain([NotNull] X509Certificate certificate);
    }
}
