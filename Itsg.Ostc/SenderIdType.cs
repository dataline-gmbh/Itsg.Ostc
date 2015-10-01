using System;

namespace Itsg.Ostc
{
    /// <summary>
    /// Art der Absender-ID
    /// </summary>
    public enum SenderIdType
    {
        /// <summary>
        /// Betriebsnummer
        /// </summary>
        BNR,
        /// <summary>
        /// IK
        /// </summary>
        IK,
        /// <summary>
        /// ZNR
        /// </summary>
        /// <remarks>
        /// War das Teil von ELENA?
        /// </remarks>
        ZNR,
    }
}
