using System;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Informationen für den OSTC-Client
    /// </summary>
    public class OstcClientInfo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="manufacturer">Hersteller</param>
        /// <param name="product">Produkt</param>
        /// <param name="registrationId">Registrierungs-ID</param>
        public OstcClientInfo(string manufacturer, string product, int registrationId)
        {
            Manufacturer = manufacturer;
            Product = product;
            RegistrationId = registrationId;
        }

        /// <summary>
        /// Hersteller
        /// </summary>
        public string Manufacturer { get; }

        /// <summary>
        /// Produkt
        /// </summary>
        public string Product { get; }

        /// <summary>
        /// Registrierungs-ID
        /// </summary>
        /// <remarks>
        /// Diese Registrierungs-ID wird von der OSTC vergeben
        /// </remarks>
        public int RegistrationId { get; }
    }
}
