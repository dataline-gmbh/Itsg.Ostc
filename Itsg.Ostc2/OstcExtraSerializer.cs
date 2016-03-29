using System.IO;
using System.Text;
using System.Xml.Serialization;

using ExtraStandard.Extra11;

using RestSharp.Portable;

namespace Itsg.Ostc2
{

    /// <summary>
    /// Spezieller <see cref="ISerializer"/>, damit die eXTra-Meldung korrekt serialisiert werden kann
    /// </summary>
    public class OstcExtraSerializer : ISerializer
    {
        private readonly XmlSerializer _serializer;

        /// <summary>
        /// Standard-Serialisierer für ISO-8859-1
        /// </summary>
        public static OstcExtraSerializer Iso88591 = new OstcExtraSerializer(OstcClient.Iso88591);

        /// <summary>
        /// Standard-Serialisierer für UTF-8
        /// </summary>
        public static OstcExtraSerializer Utf8 = new OstcExtraSerializer(Encoding.UTF8);

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="encoding">Das Encoding das für die Serialisierung der eXTra-Meldung genutzt werden soll</param>
        public OstcExtraSerializer(Encoding encoding)
        {
            Encoding = encoding;
            ContentType = "application/octet-stream";
            _serializer = new XmlSerializer(typeof(TransportRequestType));
        }

        /// <summary>
        /// Das zu verwendende Encoding für die Serialisierung der eXTra-Nachricht
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Der Content-Type, der für die Serialisierung der eXTra-Meldung angegeben werden soll
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Serialisierung eines Objekts vom Typ <see cref="TransportRequestType"/>
        /// </summary>
        /// <param name="obj">Das zu serialisierende Objekt muss vom Typ <see cref="TransportRequestType"/> sein</param>
        /// <returns>Die serialisierte eXTra-Nachricht</returns>
        public byte[] Serialize(object obj)
        {
            var output = new MemoryStream();
            var settings = new System.Xml.XmlWriterSettings()
            {
                Encoding = Encoding,
                Indent = true,
                NamespaceHandling = System.Xml.NamespaceHandling.OmitDuplicates,
            };
            using (var writer = System.Xml.XmlWriter.Create(output, settings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                namespaces.Add("xcpt", "http://www.extra-standard.de/namespace/components/1");
                namespaces.Add("xplg", "http://www.extra-standard.de/namespace/plugins/1");
                namespaces.Add("xreq", "http://www.extra-standard.de/namespace/request/1");
                _serializer.Serialize(writer, obj, namespaces);
            }
            return output.ToArray();
        }
    }
}
