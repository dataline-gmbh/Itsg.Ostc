using System;

namespace Itsg.Ostc1
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
            public static readonly Uri Production = new Uri("https://www.itsg-trust.de/ostc/");

            /// <summary>
            /// URL für den Test-Betrieb
            /// </summary>
            public static readonly Uri Test = new Uri("https://www.itsg-trust.de/ostctest/");
        }

        /// <summary>
        /// URLs relativ zu den Basis-URLs
        /// </summary>
        public static class Requests
        {
            /// <summary>
            /// URL für den Upload
            /// </summary>
            public static readonly Uri Upload = new Uri("recert-upload.php", UriKind.Relative);

            /// <summary>
            /// URL für die Ermittlung des Antrags-Status
            /// </summary>
            public static readonly Uri Order = new Uri("/all/antrag.php", UriKind.RelativeOrAbsolute);
        }
    }
}
