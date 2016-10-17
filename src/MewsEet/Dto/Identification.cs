using System;
using System.Security.Cryptography;

namespace MewsEet.Dto
{
    public class Identification
    {
        public Identification(string taxPayerIdentifier, string registryIdentifier, int premisesIdentifier, AsymmetricAlgorithm key)
            : this(taxPayerIdentifier, null, registryIdentifier, premisesIdentifier, key)
        {
        }

        public Identification(string taxPayerIdentifier, string mandatingTaxPayerIdentifier, string registryIdentifier, int premisesIdentifier, MandationType mandationType, AsymmetricAlgorithm key)
            : this(mandatingTaxPayerIdentifier, mandationType == Dto.MandationType.Section9Paragraph1 ? taxPayerIdentifier : null, registryIdentifier, premisesIdentifier, key, mandationType)
        {
        }

        private Identification(string taxPayerIdentifier, string mandantingTaxPayerIdentifier, string registryIdentifier, int premisesIdentifier, AsymmetricAlgorithm key, MandationType? mandationType = null)
        {
            if (!taxPayerIdentifier.IsTaxIdentifier() || (mandantingTaxPayerIdentifier != null && !mandantingTaxPayerIdentifier.IsTaxIdentifier()))
            {
                throw new ArgumentException($"The tax identifier is not matching the pattern '{Patterns.TaxIdentifier}'.");
            }

            if (!registryIdentifier.IsRegistryIdentifier())
            {
                throw new ArgumentException($"Registry identifier is not matching the requested pattern '{Patterns.RegistryIdentifier}'");
            }

            if (premisesIdentifier < 1 || premisesIdentifier > 999999)
            {
                throw new ArgumentException("Premises identifier is not within the expected range <1, 999999>.");
            }

            TaxPayerIdentifier = taxPayerIdentifier;
            MandantingTaxPayerIdentifier = mandantingTaxPayerIdentifier;
            RegistryIdentifier = registryIdentifier;
            PremisesIdentifier = premisesIdentifier;
            MandationType = mandationType;
            Key = key;
        }

        public string TaxPayerIdentifier { get; }

        public string MandantingTaxPayerIdentifier { get; }

        public string RegistryIdentifier { get; }

        public int PremisesIdentifier { get; }

        public MandationType? MandationType { get; }

        public AsymmetricAlgorithm Key { get; }
    }
}
