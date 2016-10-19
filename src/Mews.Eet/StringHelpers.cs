using System;
using System.Text.RegularExpressions;

namespace Mews.Eet
{
    /// <summary>
    /// This class is introduced as a replacement of StringExtensions in order to avoid polluting global space.
    /// </summary>
    public static class StringHelpers
    {
        public static bool SafeMatches(string haystack, string pattern)
        {
            if (String.IsNullOrWhiteSpace(haystack) || String.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }
            return Regex.Match(haystack, pattern).Success;
        }
    }
}
