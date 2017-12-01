using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Itsg.Ostc.Certificates
{
    /// <summary>
    /// Informationen über die gefundene Zertifikatskette
    /// </summary>
    public sealed class CertificateChain
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="CertificateChain"/> Klasse.
        /// </summary>
        /// <param name="certificate">Das Zertifikat für das die Kette gesucht wurde</param>
        /// <param name="chain">Die Zertifikatskette</param>
        internal CertificateChain(X509Certificate2 certificate, X509Certificate2[] chain)
        {
            if (chain.Length < 1)
                throw new ArgumentOutOfRangeException(nameof(chain), "Die Zertifikatskette muss mindestens das Root-Zertifikat enthalten");
            Certificate = certificate;
            RootCertificate = chain[chain.Length - 1];
            var immed = new X509Certificate2[chain.Length - 1];
            Array.Copy(chain, immed, chain.Length - 1);
            IntermediateCertificates = immed;
            Chain = chain;
        }

        /// <summary>
        /// Holt das Zertifikat für das die Kette ermittelt wurde
        /// </summary>
        public X509Certificate2 Certificate { get; }

        /// <summary>
        /// Holt das Root-Zertifikat
        /// </summary>
        public X509Certificate2 RootCertificate { get; }

        /// <summary>
        /// Holt die Zwischenzertifikate
        /// </summary>
        /// <remarks>
        /// Je kleiner der Index, desto näher ist das Zwischenzertifikat am <see cref="Certificate"/> und weiter weg vom <see cref="RootCertificate"/>.
        /// </remarks>
        public IReadOnlyCollection<X509Certificate2> IntermediateCertificates { get; }

        /// <summary>
        /// Holt die Kette der Zertifikate
        /// </summary>
        /// <remarks>
        /// Das <see cref="RootCertificate"/> ist das letzte in der Kette. Die Kette enthält nicht das <see cref="Certificate"/>.
        /// </remarks>
        public IReadOnlyCollection<X509Certificate2> Chain { get; }
    }
}
