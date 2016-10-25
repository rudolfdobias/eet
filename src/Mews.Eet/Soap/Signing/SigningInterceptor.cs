using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using Mews.Eet.Dto;
using SignSoapMessage;

namespace Mews.Eet
{
    public class SigningInterceptor : IClientMessageInspector
    {
        public SigningInterceptor(Certificate certificate)
        {
            Certificate = certificate;
        }

        private Certificate Certificate { get; }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var document = new XmlDocument();
            document.LoadXml(request.ToString());
            var revenueElement = document.GetElementsByTagName("Trzba", "http://fs.mfcr.cz/eet/schema/v3")[0];

            var soapMessage = new SoapMessage()
            {
                Body = revenueElement as XmlElement,
                Certificate = new X509Certificate2(Certificate.Data, Certificate.Password)
            };

            var signedMessageXml = soapMessage.GetXml(true);

            var newMessage = Message.CreateMessage(new XmlNodeReader(signedMessageXml), Int32.MaxValue, request.Version);
            newMessage.Properties.CopyProperties(request.Properties);
            request = newMessage;

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}