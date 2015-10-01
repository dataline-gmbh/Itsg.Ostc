namespace Itsg.Ostc1
{
    /// <summary>
    /// Validator for the OSTC certificate request
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Validate the OSTC certificate request document
        /// </summary>
        /// <param name="order">the OSTC certificate request document</param>
        void Validate(byte[] order);
    }
}
