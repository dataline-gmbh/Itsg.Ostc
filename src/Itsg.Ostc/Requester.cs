using JetBrains.Annotations;

namespace Itsg.Ostc
{
    /// <summary>
    /// Information about the requester of the certificate
    /// </summary>
    public class Requester : IRequester
    {
        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="Requester"/> Klasse.
        /// </summary>
        /// <param name="number">BN oder IK</param>
        /// <param name="companyName">Firmenname</param>
        /// <param name="surname">Nachname</param>
        public Requester([NotNull] string number, [NotNull] string companyName, [NotNull] string surname)
        {
            Number = number;
            CompanyName = companyName;
            Surname = surname;
        }

        /// <summary>
        /// Holt oder setzt die BN oder IK
        /// </summary>
        public string Number { get; }

        /// <summary>
        /// Holt oder setzt den Firmennamen
        /// </summary>
        public string CompanyName { get; }

        /// <summary>
        /// Holt oder setzt den Nachnamen
        /// </summary>
        public string Surname { get; } 
    }
}
