using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Resolvers;
using System.Xml.Schema;

namespace Itsg.Ostc1.Validator
{
    /// <summary>
    /// OSTC certificate order validator
    /// </summary>
    public class OstcAntragValidator : IValidator
    {
        /// <summary>
        /// Validate the OSTC certificate request document
        /// </summary>
        /// <param name="order">the OSTC certificate request document</param>
        public void Validate(byte[] order)
        {
            var resolver = new XmlPreloadedResolver();

            var ostcUri = "http://localhost/OSTC.xsd";
            var asm = typeof(OstcAntragValidator).Assembly;
            using (var stream = asm.GetManifestResourceStream(typeof(OstcAntragValidator), "Schemas.OSTC.xsd"))
            {
                resolver.Add(new Uri(ostcUri, UriKind.RelativeOrAbsolute), stream);
            }

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                ValidationFlags = XmlSchemaValidationFlags.ProcessSchemaLocation,
                XmlResolver = resolver,
            };

            var input = FixDocument(order, ostcUri);
            var reader = XmlReader.Create(input, settings);
            while (reader.Read())
            {
            }
        }

        private Stream FixDocument(byte[] document, string ostcUri)
        {
            var input = new MemoryStream(document);
            var doc = XDocument.Load(input);
            var xsiNs = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            doc.Root.SetAttributeValue(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance");
            doc.Root.SetAttributeValue(xsiNs + "noNamespaceSchemaLocation", ostcUri);
            var output = new MemoryStream();
            doc.Save(output);

            output.Position = 0;
            return output;
        }
    }
}
