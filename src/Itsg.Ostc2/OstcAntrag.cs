using System;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Itsg.Ostc2
{
    partial class OstcAntrag
    {
        /// <summary>
        /// XSD-Hinweis für die Antragsdaten
        /// </summary>
        [XmlAttribute(Namespace = "http://www.w3.org/2001/XMLSchema-instance", Form = XmlSchemaForm.Qualified, AttributeName = "noNamespaceSchemaLocation")]
        public string NoNamespaceSchemaLocation
        {
            get { return "../Schema/OSTC 1a-Antragsdaten.xsd"; }
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                // Diese Eigenschaft existiert nur, damit die XML-Serialisierung funktioniert
            }
        }
    }
}
