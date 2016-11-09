using System;
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
            where TIn : class, new()
            where TOut : class, new()
        {
            var messageBodyXmlElement = XmlManipulator.Serialize(messageBodyObject);
            var soapMessage = new SoapMessage(new SoapMessagePart(messageBodyXmlElement));
            var xmlDocument = Certificate == null ? soapMessage.GetXmlDocument() : soapMessage.GetSignedXmlDocument(Certificate, SignAlgorithm);
            return HttpClient.Send(xmlDocument.OuterXml, operation).ContinueWith(t => Task.FromResult(XmlManipulator.Deserialize<TOut>(GetSoapBody(t.Result)))).Unwrap();
        }

        private XmlElement GetSoapBody(string soapXmlString)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapXmlString);

            var soapMessage = SoapMessage.FromSoapXml(xmlDocument);
            if (!soapMessage.VerifySignature())
            {
                throw new Exception("The SOAP message signature is not valid.");
            }
            return soapMessage.Body.XmlElement.FirstChild as XmlElement;
        }
    }
}
