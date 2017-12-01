using System;

using Itsg.Ostc;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Nutzer des OSTC-Client (Absender)
    /// </summary>
    public class OstcSender
    {
        /// <summary>
        /// Erstellung des OSTC-Client-Nutzers (Absender) anhand des Antrags
        /// </summary>
        /// <param name="application">Antrag</param>
        public OstcSender(OstcAntrag application)
            : this(SenderId.FromBnr(application.Antragsteller.IK_BN), application.Antragsteller.Firma)
        {

        }

        /// <summary>
        /// Erstellung des OSTC-Client-Nutzers (Absender) anhand der Absender-ID und des Firmen-Namens
        /// </summary>
        /// <param name="sender">Absender-ID</param>
        /// <param name="companyName">Name der Firma</param>
        public OstcSender(SenderId sender, string companyName)
        {
            SenderId = sender;
            CompanyName = companyName;
        }

        /// <summary>
        /// Absender-ID
        /// </summary>
        public SenderId SenderId { get; private set; }

        /// <summary>
        /// Firmen-Name
        /// </summary>
        public string CompanyName { get; private set; }
    }
}
