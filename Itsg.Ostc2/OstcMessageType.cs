using System;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Meldungstyp
    /// </summary>
    public enum OstcMessageType
    {
        /// <summary>
        /// Antragstellung
        /// </summary>
        Application,

        /// <summary>
        /// Bestätigung des Antrags
        /// </summary>
        Order,

        /// <summary>
        /// Abholung des Zertifikats
        /// </summary>
        Key,

        /// <summary>
        /// Abfragen der Zertifikat-Liste
        /// </summary>
        List,

        /// <summary>
        /// Daten für die Antragstellung
        /// </summary>
        ApplicationData,

        /// <summary>
        /// Daten für die Abfrage des Auftragsstatus
        /// </summary>
        OrderData,

        /// <summary>
        /// Daten für die Abholung des Zertifikats
        /// </summary>
        KeyData,

        /// <summary>
        /// Daten für das Abfragen der Zertifikat-Liste
        /// </summary>
        ListData,
    }
}
