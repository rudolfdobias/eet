using FiscalMachines.CZ.DTO.Data;
using FiscalMachines.CZ.DTO.Service;
using FiscalMachines.CZ.EETService;

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
            var client = EETClients.Create(Environment);
            var revenueMessage = GetRevenueRequest(revenue);
            object item;
            OdpovedVarovaniType[] warnings;
            var responseHeader = client.OdeslaniTrzby(revenueMessage.Head, revenueMessage.Data, revenueMessage.CheckCodes, out item, out warnings);
            return ProcessRevenueResponse(responseHeader, item, warnings);
        }

        private EETRevenueRequest GetRevenueRequest(EETRevenue revenue)
        {
            return new EETRevenueRequest();
        }

        private EETRevenueResult ProcessRevenueResponse(OdpovedHlavickaType header, object item, OdpovedVarovaniType[] warnings)
        {
            return new EETRevenueResult();
        }
    }
}
