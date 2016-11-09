using System;

namespace Mews.Eet.Dto.Identifiers
{
    public class StringIdentifier : Identifier<string>
    {
        protected StringIdentifier(string value, string pattern)
            : base(value)
        {
            if (!StringHelpers.SafeMatches(value, pattern))
            {
                throw new ArgumentException($"The value '{value}' does not match the pattern '{pattern}'");
            }
            Pattern = pattern;
        }

        protected string Pattern { get; }
    }
}
