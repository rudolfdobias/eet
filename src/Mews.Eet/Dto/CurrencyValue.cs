using System;
using System.Globalization;

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

            if (!StringHelpers.SafeMatches(String.Format(NumberFormatInfo.InvariantInfo, "{0:F2}", value), Patterns.CurrencyValue))
            {
                throw new ArgumentException("The string representation of the currency value does not match the expected pattern, exactly 2 decimal places have to be specified.");
            }

            Value = value;
            CurrencyIsoCode = "CZK";
        }

        public string CurrencyIsoCode { get; }

        internal decimal Value { get; }
    }
}