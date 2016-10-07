using System;

namespace FiscalMachines.CZ
{
    public static class EETClients
    {
        public static EETService.EETClient Create(EETEnvironment environment)
        {
            if (environment == EETEnvironment.Production)
            {
                return new EETService.EETClient();
            }

            throw new NotImplementedException($"The environment {environment} is not currently supported.");
        }
    }
}
