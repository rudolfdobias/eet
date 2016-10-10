namespace FiscalMachines.CZ.DTO.Data
{
    public class TaxPayerRepresentation
    {
        public TaxPayerRepresentation(TaxPayer representingTaxPayer, TaxPayerRepresentationType type)
        {
            RepresentingTaxPayer = representingTaxPayer;
            Type = type;
        }

        public TaxPayerRepresentationType Type { get; }

        public TaxPayer RepresentingTaxPayer { get; }
    }
}
