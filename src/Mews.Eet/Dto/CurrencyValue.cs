using System;

namespace Mews.Eet.Dto
{
    public class CurrencyValue
    {
        public CurrencyValue(decimal value)
        {
            var decimalPlaces = BitConverter.GetBytes(Decimal.GetBits(value)[3])[2];
            if (decimalPlaces != 2)
            {
                throw new ArgumentException("EET requires the currency values to be reported with 2 decimal places.");
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