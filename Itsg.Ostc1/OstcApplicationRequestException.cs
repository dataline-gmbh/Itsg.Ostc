using System;
using System.Collections.Generic;
using System.Linq;

using Itsg.Ostc;

namespace Itsg.Ostc1
{
    /// <summary>
    /// Exception, die bei Fehlern der Antragstellung ausgelöst wird
    /// </summary>
    public class OstcApplicationRequestException : OstcException
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="errorCodes"></param>
        public OstcApplicationRequestException(int returnCode, IReadOnlyCollection<int> errorCodes)
            : base(DetermineErrorMessage(returnCode, errorCodes))
        {
            ErrorCodes = errorCodes;
            ReturnCode = returnCode;
        }

        /// <summary>
        /// Rückgabe-Code
        /// </summary>
        public int ReturnCode { get; private set; }

        /// <summary>
        /// Fehler-Codes
        /// </summary>
        public IEnumerable<int> ErrorCodes { get; private set; }

        private static string DetermineErrorMessage(int returnCode, IReadOnlyCollection<int> errorCodes)
        {
            string errorMessage = (OstcMessages.ReturnCodeMessages.ContainsKey(returnCode) ? OstcMessages.ReturnCodeMessages[returnCode] : "Unbekannter Rückgabe-Wert");
            var exceptionMessage = $"Rückgabe-Wert {returnCode}: {errorMessage}";
            if (errorCodes.Count != 0)
            {
                var errorCodesList = string.Join(", ", errorCodes.Select(x => x.ToString()).ToArray());
                exceptionMessage = $"{exceptionMessage}\nFolgende Fehlercodes wurden zurückgeliefert: {errorCodesList}";
            }
            return exceptionMessage;
        }
    }
}
