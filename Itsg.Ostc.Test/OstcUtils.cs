using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using Itsg.Ostc1;

namespace Itsg.Ostc.Test
{
    /// <summary>
    /// Hilfsfunktionen
    /// </summary>
    internal static class OstcUtils
    {
        /// <summary>
        /// Serialisiert ein OSTC-Dokument
        /// </summary>
        /// <param name="value">OSTC-Dokument</param>
        /// <param name="encoding">Zeichensatz, der für die Erstellung der XML-Datei verwendet wird</param>
        /// <returns>Serialisiertes OSTC-Dokument</returns>
        public static byte[] Serialize(OstcAntrag value, Encoding encoding)
        {
            var serializer = new XmlSerializer(typeof(OstcAntrag));
            var output = new MemoryStream();
            var writerSettings = new System.Xml.XmlWriterSettings()
            {
                Encoding = encoding,
                Indent = true,
                NamespaceHandling = System.Xml.NamespaceHandling.OmitDuplicates,
                NewLineChars = "\n",
            };
            using (var writer = System.Xml.XmlWriter.Create(output, writerSettings))
            {
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                serializer.Serialize(writer, value, namespaces);
            }
            return output.ToArray();
        }
    }
}
