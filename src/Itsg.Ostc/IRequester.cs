using JetBrains.Annotations;

namespace Itsg.Ostc
{
    /// <summary>
    /// Information about the requester of the certificate
    /// </summary>
    public interface IRequester
    {
        /// <summary>
        /// Holt oder setzt die BN oder IK
        /// </summary>
        [NotNull]
        string Number { get; }

        /// <summary>
        /// Holt oder setzt den Firmennamen
        /// </summary>
        [NotNull]
        string CompanyName { get; }

        /// <summary>
        /// Holt oder setzt den Nachnamen
        /// </summary>
        [NotNull]
        string Surname { get; }
    }
}
