using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace Itsg.Ostc
{
    /// <summary>
    /// Basis-Klasse für ein <see cref="IOstcCertificateStore"/>
    /// </summary>
    public sealed class OstcCertificateCollectionStore : IOstcCertificateStore
    {
        private readonly IReadOnlyList<X509Certificate> _receiverCertificates;
        private readonly Dictionary<ReceiverId, X509Certificate> _receiverToCertificate;

        /// <summary>
        /// Konstruktor, der Empfänger-Zertifikate übergeben bekommen muss
        /// </summary>
        /// <param name="certs">Empfänger-Zertifikate</param>
        public OstcCertificateCollectionStore(IEnumerable<X509Certificate> certs)
        {
            _receiverCertificates = certs.ToList();
            _receiverToCertificate = BuildDictionary(_receiverCertificates);
        }

        /// <summary>
        /// Das aktuelle Zertifikat für den angegebenen Empfänger
        /// </summary>
        /// <param name="receiverId">Empfänger-ID</param>
        /// <returns>Das aktuelle Zertifikat für den Empfänger oder null</returns>
        public X509Certificate GetCertificate(ReceiverId receiverId)
        {
            X509Certificate result;
            if (!_receiverToCertificate.TryGetValue(receiverId, out result))
                return null;
            return result;
        }

        /// <summary>
        /// Aufbau einer Zertifikatskette
        /// </summary>
        /// <param name="certificate">Das Zertifikat für das die Zertifikatskette aufgebaut werden soll</param>
        /// <returns>Zertifikate für die Zertifikatskette</returns>
        /// <remarks>Es wird davon ausgegangen, dass alle notwendigen Zertifikate für die Zertifikatskette in den Empfänger-Zertifikaten enthalten sind.</remarks>
        public IReadOnlyCollection<X509Certificate> GetChain(X509Certificate certificate)
        {
            return GetChain(certificate, _receiverCertificates);
        }

        /// <summary>
        /// Aufbau eines Dictionary, das für einem Empfänger auf ein Empfänger-Zertifikat verweist
        /// </summary>
        /// <param name="certificates">Liste von Empfänger-Zertifikaten</param>
        /// <returns>Das Dictionary</returns>
        /// <remarks>Diese Funktion wird aufgerufen, wenn die Eigenschaft <see cref="F:_receiverCertificates"/> gesetzt wird.</remarks>
        private static Dictionary<ReceiverId, X509Certificate> BuildDictionary(IEnumerable<X509Certificate> certificates)
        {
            var result = new Dictionary<ReceiverId, X509Certificate>();
            if (certificates != null)
            {
                foreach (var cert in certificates)
                {
                    var name = cert.SubjectDN;
                    var values = name.GetValueList(Org.BouncyCastle.Asn1.X509.X509Name.OU);
                    if (values.Count == 0)
                        continue;
                    var receiverId = ReceiverId.FromBnrOrIk((string)values[0]);
                    result.Add(receiverId, cert);
                }
            }
            return result;
        }

        /// <summary>
        /// Aufbau einer Zertifikatskette
        /// </summary>
        /// <param name="cert">Das Zertifikat für das die Zertifikatskette aufgebaut werden soll</param>
        /// <param name="certs">Zertifikate, die für den Aufbau der Zeritifkatskette notwendig sind</param>
        /// <returns>Zertifikate für die Zertifikatskette</returns>
        /// <exception cref="System.ArgumentException">Wird ausgeworfen, wenn keine Zertifikatskette aufgebaut werden kann</exception>
        [NotNull]
        private static IReadOnlyCollection<X509Certificate> GetChain([NotNull] X509Certificate cert, [CanBeNull] IReadOnlyList<X509Certificate> certs)
        {
            var certList = new List<X509Certificate>();
            if (certs != null)
                certList.AddRange(certs);
            certList.Add(cert);

            var certStore = X509StoreFactory.Create("Certificate/Collection", new X509CollectionStoreParameters(certList));

            var rootCerts = certs.Where(IsSelfSigned).ToList();
            var trustAnchors = rootCerts.Select(x => new TrustAnchor(x, null));
            var trust = new HashSet(trustAnchors);

            var cpb = new PkixCertPathBuilder();
            var targetConstraints = new X509CertStoreSelector()
            {
                Certificate = cert,
            };
            var parameters = new PkixBuilderParameters(trust, targetConstraints)
            {
                IsRevocationEnabled = false,
            };
            parameters.AddStore(certStore);
            var cpbResult = cpb.Build(parameters);

            var result = new List<X509Certificate>();
            result.AddRange(cpbResult.CertPath.Certificates.Cast<X509Certificate>());
            result.Add(cpbResult.TrustAnchor.TrustedCert);
            return result;
        }

        private static bool IsSelfSigned(X509Certificate certificate)
        {
            if (!certificate.SubjectDN.Equivalent(certificate.IssuerDN))
                return false;
            try
            {
                certificate.Verify(certificate.GetPublicKey());
                return true;
            }
            catch (SignatureException)
            {
                return false;
            }
            catch (InvalidKeyException)
            {
                return false;
            }
        }
    }
}
