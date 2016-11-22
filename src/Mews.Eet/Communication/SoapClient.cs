using System;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml;

namespace Mews.Eet.Communication
{
    public class SoapClient
    {
        public SoapClient(Uri endpointUri, X509Certificate2 certificate, SignAlgorithm signAlgorithm = SignAlgorithm.Sha256, EetLogger logger = null)
        {
            HttpClient = new SoapHttpClient(endpointUri);
            Certificate = certificate;
            SignAlgorithm = signAlgorithm;
            XmlManipulator = new XmlManipulator();
            Logger = logger;
        }

        private SoapHttpClient HttpClient { get; }

        private X509Certificate2 Certificate { get; }

        private SignAlgorithm SignAlgorithm { get; }

        private XmlManipulator XmlManipulator { get; }

        private EetLogger Logger { get; }

        public async Task<TOut> SendAsync<TIn, TOut>(TIn messageBodyObject, string operation, TimeSpan httpTimeout)
            where TIn : class, new()
            where TOut : class, new()
        {
            var messageBodyXmlElement = XmlManipulator.Serialize(messageBodyObject);
            var soapMessage = new SoapMessage(new SoapMessagePart(messageBodyXmlElement));
            var xmlDocument = Certificate == null ? soapMessage.GetXmlDocument() : soapMessage.GetSignedXmlDocument(Certificate, SignAlgorithm);

            var xml = xmlDocument.OuterXml;
            Logger?.Debug($"Ready to send Signed XML to EET servers.", xml);

            var response = await HttpClient.SendAsync(xml, operation, httpTimeout).ConfigureAwait(continueOnCapturedContext: false);
            Logger?.Debug($"Received response from EET servers.", response);

            var soapBody = GetSoapBody(response);
            return XmlManipulator.Deserialize<TOut>(soapBody);
        }

        private XmlElement GetSoapBody(string soapXmlString)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(soapXmlString);

            var soapMessage = SoapMessage.FromSoapXml(xmlDocument);
            if (!soapMessage.VerifySignature())
            {
                throw new SecurityException("The SOAP message signature is not valid.");
            }
            return soapMessage.Body.XmlElement.FirstChild as XmlElement;
        }
    }
}
