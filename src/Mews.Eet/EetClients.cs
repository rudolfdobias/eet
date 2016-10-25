using System;
using System.ServiceModel;
using Mews.Eet.Dto;

namespace Mews.Eet
{
    internal static class EetClients
    {
        internal static EetService.EETClient Create(EetEnvironment environment, Identification identification)
        {
            var address = new EndpointAddress(new Uri("https://pg.eet.cz:443/eet/services/EETServiceSOAP/v3"));
            var client = new EetService.EETClient(new BasicHttpBinding(BasicHttpSecurityMode.Transport), address);
            client.Endpoint.Behaviors.Add(new SigningBehavior(identification.Certificate));
            return client;
        }
    }
}
