using System;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Kommunikationsspezifische URLs
    /// </summary>
    public static class Network
    {
        /// <summary>
        /// Basis-URLs
        /// </summary>
        public static class Base
        {
            /// <summary>
            /// URL für den Echt-Betrieb
            /// </summary>
            public static readonly Uri Production = new Uri("https://www.itsg-trust.de/ostcv2/");

            /// <summary>
            /// URL für den Test-Betrieb
            /// </summary>
            public static readonly Uri Test = new Uri("https://www.itsg-trust.de/ostcv2test/");
        }

        /// <summary>
        /// URLs relativ zu den Basis-URLs
        /// </summary>
        public static class Requests
        {
            /// <summary>
            /// URL für die Antragstellung
            /// </summary>
            public static readonly Uri Application = new Uri("antrag.php", UriKind.Relative);

            /// <summary>
            /// URL für die Bestätigung des Antrags
            /// </summary>
            public static readonly Uri Order = new Uri("auftrag.php", UriKind.Relative);

            /// <summary>
            /// URL für die Abfrage des erstellten Absender-Zertifikats
            /// </summary>
            public static readonly Uri KeyRequest = new Uri("schluessel.php", UriKind.Relative);

            /// <summary>
            /// URL für die Abfrage der Zertifikat-Listen
            /// </summary>
            public static readonly Uri ListRequest = new Uri("liste.php", UriKind.Relative);
        }
    }
}
