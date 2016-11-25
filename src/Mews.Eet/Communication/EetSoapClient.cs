using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Wsdl;

namespace Mews.Eet.Communication
{
    public class EetSoapClient
    {
        static EetSoapClient()
        {
            CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
        }

        public EetSoapClient(Certificate certificate, EetEnvironment environment, TimeSpan httpTimeout, EetLogger logger = null)
        {
            Environment = environment;
            var subdomain = environment == EetEnvironment.Production ? "prod" : "pg";
            var endpointUri = new Uri($"https://{subdomain}.eet.cz:443/eet/services/EETServiceSOAP/v3");
            SoapClient = new SoapClient(endpointUri, certificate.X509Certificate2, httpTimeout, SignAlgorithm.Sha256, logger);
            Logger = logger;
        }

        public EetEnvironment Environment { get; }

        private SoapClient SoapClient { get; }

        private EetLogger Logger { get; }

        public async Task<SendRevenueXmlResponse> SendRevenueAsync(SendRevenueXmlMessage message)
        {
            return await SoapClient.SendAsync<SendRevenueXmlMessage, SendRevenueXmlResponse>(message, operation: "http://fs.mfcr.cz/eet/OdeslaniTrzby").ConfigureAwait(continueOnCapturedContext: false);
        }
    }
}
