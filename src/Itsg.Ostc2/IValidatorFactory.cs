using ExtraStandard.Validation;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Validator factory for the validation of OSTC request documents
    /// </summary>
    public interface IValidatorFactory
    {
        /// <summary>
        /// Creates an eXTra standard validator for OSTC request documents
        /// </summary>
        /// <param name="messageType">The OSTC message type</param>
        /// <param name="transportDirection">Transport direction</param>
        /// <returns>the new validator</returns>
        IExtraValidator Create(OstcMessageType messageType, ExtraTransportDirection transportDirection);
    }
}
