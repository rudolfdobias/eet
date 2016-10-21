using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using Mews.Eet.Dto;

namespace Mews.Eet
{
    internal static class EetClients
    {
        internal static EetService.EETClient Create(EetEnvironment environment, Identification identification)
        {
            var binding = new BasicHttpsBinding
            {
                Security = new BasicHttpsSecurity()
                {
                    Message = new BasicHttpMessageSecurity()
                    {
                        AlgorithmSuite = SecurityAlgorithmSuite.Basic128Sha256Rsa15,
                        ClientCredentialType = BasicHttpMessageCredentialType.Certificate
                    },
                    Mode = BasicHttpsSecurityMode.TransportWithMessageCredential
                }
            };

            var client = new EetService.EETClient(binding, new EndpointAddress("https://pg.eet.cz:443/eet/services/EETServiceSOAP/v3"));
            client.ClientCredentials.ClientCertificate.Certificate = new X509Certificate2(identification.Certificate.Data, identification.Certificate.Password);
            var elements = client.Endpoint.Binding.CreateBindingElements();
            elements.Find<SecurityBindingElement>().EnableUnsecuredResponse = true;
            client.Endpoint.Binding = new CustomBinding(elements);
            return client;
        }
    }
}
