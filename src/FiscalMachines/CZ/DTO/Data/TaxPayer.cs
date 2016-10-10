namespace FiscalMachines.CZ.DTO.Data
{
    public class TaxPayer
    {
        public TaxPayer(string taxIdentifier, TaxPayerRepresentation taxPayerRepresentation = null)
        {
            TaxIdentifier = taxIdentifier;
            Representation = taxPayerRepresentation;
        }

        public string TaxIdentifier { get; }

        public TaxPayerRepresentation Representation { get; }
    }
}
