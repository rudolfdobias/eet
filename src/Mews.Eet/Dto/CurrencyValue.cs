using System;

namespace Mews.Eet.Dto
{
    public class CurrencyValue
    {
        public CurrencyValue(decimal value)
        {
            if (Decimal.Round(value, 2) != value)
            {
                throw new ArgumentException("The currency value cannot be more precise than 2 decimal places.");
            }

            if (value > 99999999.99m)
            {
                throw new ArgumentException("The value cannot be higher than 99 999 999,99 Kč.");
            }

            if (value < -99999999.99m)
            {
                throw new ArgumentException("The value cannot be lower than -99 999 999,99 Kč.");
            }

            Value = value;
            CurrencyIsoCode = "CZK";
        }

        public string CurrencyIsoCode { get; }

        internal decimal Value { get; }
    }
}