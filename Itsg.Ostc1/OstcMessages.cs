using System;
using System.Collections.Generic;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Meldungen von der OSTC
    /// </summary>
    public static class OstcMessages
    {
        /// <summary>
        /// Dictionary für Rückgabe-Code zu Meldung
        /// </summary>
        public static readonly Dictionary<int, string> ReturnCodeMessages = new Dictionary<int, string>()
        {
            { 10, "Erstantrag erfolgreich" },
            { 11, "Folgeantrag erfolgreich" },
            { 12, "Folgeantrag mit Status Erstantrag" },
            { 90, "Antrag fehlerhaft oder defekt" },
            { 91, "Doppelter Antrag" },
            { 92, "Antrag in Sperrliste für Annahmestellen" },
            { 93, "Antrag in Sperrliste für gesperrte Kunden" },
            { 94, "Antrag in Sperrliste für Dakota-Lizenzen" },
            { 95, "Fehlerhafte Daten" },
            { 99, "System nicht verfügbar" },
        };
    }
}
