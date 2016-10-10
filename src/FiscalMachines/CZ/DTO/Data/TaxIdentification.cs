namespace FiscalMachines.CZ.DTO.Data
{
    public class TaxIdentification
    {
        public TaxIdentification(TaxPayer payer, string registerIdentifier, int premisesIdentifier)
        {
            Payer = payer;
            RegisterIdentifier = registerIdentifier;
            PremisesIdentifier = premisesIdentifier;
        }

        public TaxPayer Payer { get; }
        public string RegisterIdentifier { get; }
        public int PremisesIdentifier { get; }
    }
}
