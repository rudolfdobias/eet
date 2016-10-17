using System.Text.RegularExpressions;

namespace MewsEet
{
    public static class StringExtensions
    {
        public static bool IsBillNumber(this string s)
        {
            return Matches(s, Patterns.BillNumber);
        }

        public static bool IsTaxIdentifier(this string s)
        {
            return Matches(s, Patterns.TaxIdentifier);
        }

        public static bool IsUUID(this string s)
        {
            return Matches(s, Patterns.UUID);
        }

        public static bool IsRegistryIdentifier(this string s)
        {
            return Matches(s, Patterns.RegistryIdentifier);
        }

        public static bool IsCurrencyValue(this string s)
        {
            return Matches(s, Patterns.CurrencyValue);
        }

        private static bool Matches(string haystack, string pattern)
        {
            if (haystack == null || pattern == null)
            {
                return false;
            }
            return Regex.Match(haystack, pattern).Success;
        }
    }
}
