using FiscalMachines.CZ.EETService;

namespace FiscalMachines.CZ.DTO.Service
{
    public class EETRevenueRequest
    {
        public EETRevenueRequest()
        {
            
        }

        public TrzbaHlavickaType Head { get; }
        public TrzbaDataType Data { get; }
        public TrzbaKontrolniKodyType CheckCodes { get; }

    }
}
