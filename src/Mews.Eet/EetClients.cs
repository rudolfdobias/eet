using System;
using System.ServiceModel;

namespace Mews.Eet
{
    internal static class EetClients
    {
        internal static EetService.EETClient Create(EetEnvironment environment)
        {
            if (environment == EetEnvironment.Production)
            {
                return new EetService.EETClient(new BasicHttpsBinding(BasicHttpsSecurityMode.Transport), new EndpointAddress("https://pg.eet.cz:443/eet/services/EETServiceSOAP/v3"));
            }

            throw new NotImplementedException($"The environment {environment} is not currently supported.");
        }
    }
}
