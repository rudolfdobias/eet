namespace Mews.Eet.Dto
{
    public class Revenue
    {
        public Revenue(CurrencyValue gross, DateTimeWithTimeZone accepted = null, CurrencyValue notTaxable = null, TaxRateItem lowerTaxRate = null, TaxRateItem reducedTaxRate = null, TaxRateItem standardTaxRate = null, CurrencyValue travelServices = null, CurrencyValue deposit = null, CurrencyValue usedDeposit = null)
        {
            Accepted = accepted ?? DateTimeProvider.Now;
            Gross = gross;
            NotTaxable = notTaxable;
            LowerTaxRate = lowerTaxRate;
            ReducedTaxRate = reducedTaxRate;
            StandardTaxRate = standardTaxRate;
            TravelServices = travelServices;
            Deposit = deposit;
            UsedDeposit = usedDeposit;
        }

        public DateTimeWithTimeZone Accepted { get; }

        public CurrencyValue Gross { get; }

        public CurrencyValue NotTaxable { get; }

        public TaxRateItem LowerTaxRate { get; }

        public TaxRateItem ReducedTaxRate { get; }

        public TaxRateItem StandardTaxRate { get; }

        public CurrencyValue TravelServices { get; }

        public CurrencyValue Deposit { get; }

        public CurrencyValue UsedDeposit { get; }
    }
}
