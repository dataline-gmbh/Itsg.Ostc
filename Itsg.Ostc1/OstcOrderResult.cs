using System;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Status des Auftrags
    /// </summary>
    public class OstcOrderResult
    {
        /// <summary>
        /// Status
        /// </summary>
        public OstcOrderStatus Status { get; set; }
        /// <summary>
        /// URL von der das Zertifikat geladen werden kann
        /// </summary>
        public Uri DownloadUrl { get; set; }
        /// <summary>
        /// Zusätzliche Meldung bei einem Fehler
        /// </summary>
        public string Message { get; set; }
    }
}
