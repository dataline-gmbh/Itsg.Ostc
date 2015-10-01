using System.Diagnostics.Contracts;
using System.IO;
using System.Xml.Serialization;

using ExtraStandard.Extra11;

using RestSharp.Portable;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Spezieller <see cref="IDeserializer"/>, damit die eXTra-Rückmeldung korrekt deserialisiert werden kann
    /// </summary>
    public class OstcExtraDeserializer : IDeserializer
    {
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public OstcExtraDeserializer()
        {
            _serializer = new XmlSerializer(typeof(TransportResponseType));
        }

        /// <summary>
        /// Deserialisierung der eXTra-Rückmeldung
        /// </summary>
        /// <typeparam name="T">Muss <see cref="TransportResponseType"/> sein</typeparam>
        /// <param name="response">Ergebnis des Request</param>
        /// <returns>Deserialisiertes Objekt</returns>
        public T Deserialize<T>(IRestResponse response)
        {
            Contract.Requires(typeof(T) == typeof(TransportResponseType));

            var input = new MemoryStream(response.RawBytes);
            return (T)_serializer.Deserialize(input);
        }
    }
}
