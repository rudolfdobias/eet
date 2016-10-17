using System;
using System.Globalization;

namespace MewsEet.Dto
{
    public class CurrencyValue
    {
        public CurrencyValue(decimal value)
        {
            if (decimal.Round(value, 2) != value)
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

            if (decimal.Round(value, 2) != value)
            {
                throw new ArgumentException("The currency value cannot be more precise than 2 decimal places.");
            }

            if (!string.Format(NumberFormatInfo.InvariantInfo, "{0:F2}", value).IsCurrencyValue())
            {
                throw new ArgumentException("The string representation of the currency value does not match the expected pattern, exactly 2 decimal places have to be specified.");
            }

            Value = value;
            CurrencyIsoCode = "CZK";
        }

        internal decimal Value { get; }

        public string CurrencyIsoCode { get; }
    }

    public static class CurrencyValueExtensions
    {
        public static bool IsDefined(this CurrencyValue item)
        {
            return item != null;
        }

        public static decimal SafeValue(this CurrencyValue value)
        {
            if (value == null)
            {
                return default(decimal);
            }

            return value.Value;
        }
    }

}
