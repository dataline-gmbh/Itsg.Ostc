using ExtraStandard.Validation;

namespace Itsg.Ostc2.Validator
{
    /// <summary>
    /// Standard-Implementation eines <see cref="ExtraValidator"/> für OSTC-eXTra 1.1-Dokumente
    /// </summary>
    public class OstcExtraValidator : ResourceExtraValidator
    {
        /// <summary>
        /// The factory for the OSTC validators
        /// </summary>
        public static IValidatorFactory Factory => new OstcExtraValidatorFactory();

        /// <summary>
        /// Initialisiert eine neue Instanz der <see cref="OstcExtraValidator"/> Klasse.
        /// </summary>
        /// <param name="messageType">Die Meldungsart die zu prüfen ist</param>
        /// <param name="transportDirection">Gesendete oder empfangene Meldung?</param>
        public OstcExtraValidator(OstcMessageType messageType, ExtraTransportDirection transportDirection)
            : base(new OstcExtraValidationResources(messageType, transportDirection))
        {
        }

        private class OstcExtraValidatorFactory : IValidatorFactory
        {
            public IExtraValidator Create(OstcMessageType messageType, ExtraTransportDirection transportDirection)
            {
                return new OstcExtraValidator(messageType, transportDirection);
            }
        }
    }
}
