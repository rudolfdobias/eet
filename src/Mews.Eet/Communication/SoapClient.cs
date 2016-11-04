using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace Mews.Eet.Communication
{
    public class SoapClient
    {
        public SoapClient(string endpointUrl, X509Certificate2 certificate, SignAlgorithm signAlgorithm = SignAlgorithm.Sha256)
        {
            HttpClient = new SoapHttpClient(endpointUrl);
            Certificate = certificate;
            SignAlgorithm = signAlgorithm;
            XmlManipulator = new XmlManipulator();
        }

        private SoapHttpClient HttpClient { get; }

        private X509Certificate2 Certificate { get; }

        private SignAlgorithm SignAlgorithm { get; }

        private XmlManipulator XmlManipulator { get; }

        public Task<TOut> Send<TIn, TOut>(TIn messageBodyObject, string operation)
        {
            var messageBodyXmlElement = XmlManipulator.Serialize(messageBodyObject);
            var soapMessage = new SoapMessage(new SoapMessagePart(messageBodyXmlElement));
            var xmlDocument = Certificate == null ? soapMessage.GetXmlDocument() : soapMessage.GetSignedXmlDocument(Certificate, SignAlgorithm);
            return AsyncHelpers.SafeContinuationAction(HttpClient.Send(xmlDocument.OuterXml, operation), response => XmlManipulator.Deserialize<TOut>(GetSoapBody(response)));
        }

        private XmlElement GetSoapBody(string soapXmlString)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapXmlString);

            var soapMessage = SoapMessage.FromSoapXml(xmlDocument);
            return soapMessage.Body.XmlElement;
        }
    }
}
