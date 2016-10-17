using System;

namespace MewsEet.Dto
{
    public class TaxRateItem
    {
        public TaxRateItem(CurrencyValue net, CurrencyValue tax, CurrencyValue goods)
        {
            Net = net;
            Tax = tax;
            Goods = goods;

            if (Net.IsDefined() != tax.IsDefined())
            {
                throw new ArgumentException("Both tax and net should be defined or undefined.");
            }
        }

        public CurrencyValue Net { get; }

        public CurrencyValue Tax { get; }

        public CurrencyValue Goods { get; }
    }

    public static class TaxRateItemExtensions
    {
        public static bool IsDefined(this TaxRateItem item)
        {
            return item != null;
        }

        public static bool IsDefined(this TaxRateItem item, Func<TaxRateItem, CurrencyValue> valueSelector)
        {
            return item != null && valueSelector(item) != null;
        }

        public static decimal SafeValue(this TaxRateItem item, Func<TaxRateItem, CurrencyValue> valueSelector)
        {
            return IsDefined(item, valueSelector) ? valueSelector(item).SafeValue() : default(decimal);
        }
    }
}
