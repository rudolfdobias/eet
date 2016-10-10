using System;

namespace FiscalMachines.CZ.DTO.Data
{
    public class Revenue
    {
        public Revenue()
        {
            Identifier = Guid.NewGuid();
        }

        public Guid Identifier { get; }
        public DateTime Created { get; set; }
        public decimal? NotTaxableNet { get; }
        public decimal Total { get; }
        public string BillNumber { get; }
        public TaxItem LowerReducedTaxItem { get; }
        public TaxItem ReducedTaxItem { get; }
        public TaxItem StandardTaxItem { get; }
    }
}
