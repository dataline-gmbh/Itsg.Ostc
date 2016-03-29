using System;
using System.Reflection;

using ExtraStandard;
using ExtraStandard.Validation;

namespace Itsg.Ostc2.Validator
{
    internal class OstcExtraValidationResources : IExtraValidationResources
    {
        public OstcExtraValidationResources(OstcMessageType messageType, ExtraTransportDirection transportDirection)
        {
            var type = GetType();
#if HAS_FULL_TYPE
            ResourceAssembly = type.Assembly;
#else
            ResourceAssembly = type.GetTypeInfo().Assembly;
#endif
            RootUrl = new Uri($"res:///{type.Namespace?.Replace('.', '/')}/Schemas/");
            StartXmlSchemaFileName = GetXsdFileName(messageType, transportDirection);
        }

        /// <summary>
        /// Holt die Basis-URL von der ausgehend alle XSD-Dateien aus den Ressourcen geladen werden
        /// </summary>
        public Uri RootUrl { get; }

        /// <summary>
        /// Holt die Assembly aus der die XSD-Dateien aus den Ressourcen geladen werden
        /// </summary>
        public Assembly ResourceAssembly { get; }

        /// <summary>
        /// Holt den Namen der XSD die als Start für die XML-Valdierung genutzt wird.
        /// </summary>
        public string StartXmlSchemaFileName { get; }

        private static string GetXsdFileName(OstcMessageType messageType, ExtraTransportDirection transportDirection)
        {
            int methodId;
            string src;
            switch (messageType)
            {
                case OstcMessageType.Application:
                    src = "Antrag";
                    methodId = 1;
                    break;
                case OstcMessageType.Order:
                    src = "Auftrag";
                    methodId = 2;
                    break;
                case OstcMessageType.Key:
                    src = "SchluesselAnfragen";
                    methodId = 3;
                    break;
                case OstcMessageType.List:
                    src = "ListeAnfragen";
                    methodId = 4;
                    break;
                default:
                    if (transportDirection == ExtraTransportDirection.Request)
                    {
                        switch (messageType)
                        {
                            case OstcMessageType.ApplicationData:
                                return "OSTC 1a-Antragsdaten.xsd";
                            case OstcMessageType.OrderData:
                                return "OSTC 2a-Auftragsdaten.xsd";
                            case OstcMessageType.KeyData:
                                return "OSTC 3a-Schlüsseldaten.xsd";
                        }
                    }
                    throw new NotSupportedException($"The combination {messageType}/{transportDirection} is not supported yet.");
            }
            return string.Format("OSTC-{0}{2}-{1}-1.xsd", methodId, src, (transportDirection == ExtraTransportDirection.Request ? "a-request" : "b-response"));
        }
    }
}
