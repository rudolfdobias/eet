using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Mews.Eet.Dto;

namespace Mews.Eet
{
    public class SigningBehavior : IEndpointBehavior
    {
        public SigningBehavior(Certificate certificate)
        {
            Certificate = certificate;
        }

        private Certificate Certificate { get; }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new SigningInterceptor(Certificate));
        }
    }
}
