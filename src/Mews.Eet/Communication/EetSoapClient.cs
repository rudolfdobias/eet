using System.Threading.Tasks;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Wsdl;

namespace Mews.Eet.Communication
{
    public class EetSoapClient
    {
        public EetSoapClient(Certificate certificate, EetEnvironment environment)
        {
            var subdomain = environment == EetEnvironment.Production ? "prod" : "pg";
            SoapClient = new SoapClient($"https://{subdomain}.eet.cz:443/eet/services/EETServiceSOAP/v3", certificate.X509Certificate2);
        }

        private SoapClient SoapClient { get; }

        public Task<SendRevenueXmlResponse> SendRevenue(SendRevenueXmlMessage message)
        {
            return SoapClient.Send<SendRevenueXmlMessage, SendRevenueXmlResponse>(message, operation: "http://fs.mfcr.cz/eet/OdeslaniTrzby");
        }
    }
}
