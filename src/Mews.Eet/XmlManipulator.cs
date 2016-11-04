using System;
using System.Xml;
using System.Xml.Serialization;

namespace Mews.Eet
{
    public class XmlManipulator
    {
        public T Deserialize<T>(XmlElement soapBody)
        {
            return default(T);
        }

        public XmlElement Serialize<T>(T value)
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
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(writer, value);
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
