using System;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Status des Auftrags
    /// </summary>
    public enum OstcOrderStatus
    {
        /// <summary>
        /// Auftrag nicht gefunden
        /// </summary>
        /// <remarks>
        /// Das kann auch passieren, wenn der Status zu schnell nach der Antragstellung abgefragt wird. Es sollte
        /// mindestens mehrere Minuten gewartet, bis der Status abgefragt werden sollte.
        /// </remarks>
        NotFound,

        /// <summary>
        /// Status unbekannt
        /// </summary>
        /// <remarks>
        /// Dies sollte eigentlich nie zurückgeliefert werden, ausser wenn die OSTC Änderungen an der Schnittstelle in der
        /// Version 1 vorgenommen hat.
        /// </remarks>
        Unknown,

        /// <summary>
        /// Antrag wird noch nicht bearbeitet
        /// </summary>
        /// <remarks>
        /// Das passiert immer dann, wenn noch Daten an die OSTC gesandt werden müssen.
        /// </remarks>
        NotProcessed,

        /// <summary>
        /// Antrag wird bearbeitet
        /// </summary>
        Processing,

        /// <summary>
        /// Bearbeitung des Antrags ist fehlgeschlagen
        /// </summary>
        Failed,

        /// <summary>
        /// Bearbeitung des Antrags war erfolgreich
        /// </summary>
        Successful,
    }
}
