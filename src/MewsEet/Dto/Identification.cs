using System;
using System.Security.Cryptography;

namespace MewsEet.Dto
{
    public class Identification
    {
        public Identification(string taxPayerIdentifier, string registryIdentifier, int premisesIdentifier, Certificate certificate)
            : this(taxPayerIdentifier, null, registryIdentifier, premisesIdentifier, certificate)
        {
        }

        public Identification(string taxPayerIdentifier, string mandatingTaxPayerIdentifier, string registryIdentifier, int premisesIdentifier, MandationType mandationType, Certificate certificate)
            : this(mandatingTaxPayerIdentifier, mandationType == Dto.MandationType.Section9Paragraph1 ? taxPayerIdentifier : null, registryIdentifier, premisesIdentifier, certificate, mandationType)
        {
        }

        private Identification(string taxPayerIdentifier, string mandantingTaxPayerIdentifier, string registryIdentifier, int premisesIdentifier, Certificate certificate, MandationType? mandationType = null)
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

            if (certificate == null)
            {
                throw new ArgumentException("The certificate cannot be null.");
            }

            TaxPayerIdentifier = taxPayerIdentifier;
            MandantingTaxPayerIdentifier = mandantingTaxPayerIdentifier;
            RegistryIdentifier = registryIdentifier;
            PremisesIdentifier = premisesIdentifier;
            MandationType = mandationType;
            Certificate = certificate;
        }

        public string TaxPayerIdentifier { get; }

        public string MandantingTaxPayerIdentifier { get; }

        public string RegistryIdentifier { get; }

        public int PremisesIdentifier { get; }

        public MandationType? MandationType { get; }

        public Certificate Certificate { get; }
    }
}
