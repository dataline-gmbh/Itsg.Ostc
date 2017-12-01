using System;

namespace Itsg.Ostc
{
    /// <summary>
    /// Basis-Exception für OSTC-Fehler
    /// </summary>
    public class OstcException : Exception
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="message"></param>
        public OstcException(string message)
            : base(message)
        {

        }
    }
}
