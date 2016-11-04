using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Mews.Eet
{
    public class XmlManipulator
    {
        public T Deserialize<T>(XmlElement soapBody)
            where T : class, new()
        {
            using (var reader = new StringReader(soapBody.OuterXml))
            {
                var xmlSerializer = new XmlSerializer(typeof(T));
                return xmlSerializer.Deserialize(reader) as T;
            }
        }

        public XmlElement Serialize<T>(T value)
            where T : class, new()
        {
            if (value == null)
            {
                throw new ArgumentException("Cannot serialize null.");
            }

            try
            {
                var xmlDocument = new XmlDocument();
                var navigator = xmlDocument.CreateNavigator();
                using (var writer = navigator.AppendChild())
                {
                    var namespaces = new XmlSerializerNamespaces();

                    // Add an empty namespace and empty value
                    namespaces.Add("", "http://fs.mfcr.cz/eet/schema/v3");
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(writer, value, namespaces);
                }
                return xmlDocument.DocumentElement;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred during serialization.", ex);
            }
        }
    }
}
