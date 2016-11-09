using System;
using System.Xml;

namespace Mews.Eet.Communication
{
    public class SoapMessagePart
    {
        public SoapMessagePart(string bodyXml)
        {
            if (String.IsNullOrEmpty(bodyXml))
            {
                throw new ArgumentException("Content of SOAP message part cannot be empty.");
            }

            var document = new XmlDocument();
            document.LoadXml(bodyXml);
            XmlElement = document.DocumentElement;
        }

        public SoapMessagePart(XmlElement xmlElement)
        {
            XmlElement = xmlElement;
        }

        public XmlElement XmlElement { get; }
    }
}
