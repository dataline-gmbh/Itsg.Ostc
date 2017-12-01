using System;

namespace Itsg.Ostc2
{
    /// <summary>
    /// OSTC-Datentyp für die eXTra-Meldung
    /// </summary>
    public static class OstcDataType
    {
        /// <summary>
        /// Antragstellung
        /// </summary>
        public static readonly string Application = "http://www.itsg.de/ostc/Antrag";

        /// <summary>
        /// Bestätigung des Antrags
        /// </summary>
        public static readonly string Order = "http://www.itsg.de/ostc/Auftrag";

        /// <summary>
        /// Abholung des Zertifikats
        /// </summary>
        public static readonly string Key = "http://www.itsg.de/ostc/SchlüsselAnfragen";

        /// <summary>
        /// Laden der Zertifikat-Listen
        /// </summary>
        public static readonly string ListRequest = "http://www.itsg.de/ostc/ListenAnfragen";
    }
}
