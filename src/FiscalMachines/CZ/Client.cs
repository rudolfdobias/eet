using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiscalMachines.CZ
{
    public class Client
    {
        private EETEnvironment Environment { get; }

        public Client(EETEnvironment environment)
        {
            Environment = environment;
        }

        public EETRevenueResult SendRevenue(EETRevenue revenue)
        {
            var client = EETClients.Create(Environment)
        }
    }
}
