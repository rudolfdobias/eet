using System;

namespace Mews.Eet
{
    internal static class EetClients
    {
        internal static EetService.EETClient Create(EetEnvironment environment)
        {
            if (environment == EetEnvironment.Production)
            {
                return new EetService.EETClient();
            }

            throw new NotImplementedException($"The environment {environment} is not currently supported.");
        }
    }
}
