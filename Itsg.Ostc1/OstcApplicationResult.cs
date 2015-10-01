using System;
using System.Collections.Generic;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Ergebnis der Antragstellung
    /// </summary>
    public class OstcApplicationResult
    {
        /// <summary>
        /// Rückgabe-Code
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// Fehler-Codes
        /// </summary>
        public IReadOnlyCollection<int> ErrorCodes { get; set; }

        /// <summary>
        /// ID des Auftrags (wenn erfolgreich)
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// Prüft, ob der Antrag fehlerhaft war
        /// </summary>
        /// <exception cref="OstcApplicationRequestException">Wird ausgelöst, wenn bei der Antragstellung ein Fehler auftrat</exception>
        public void Validate()
        {
            switch (ReturnCode)
            {
                case 10:
                case 11:
                case 12:
                    break;
                default:
                    throw new OstcApplicationRequestException(ReturnCode, ErrorCodes);
            }
        }
    }
}
