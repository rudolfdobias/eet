using System;
using System.Security.Cryptography;
using System.Text;
using Mews.Eet.Dto.Identifiers;

namespace Mews.Eet.Dto
{
    public class RevenueRecord
    {
        public RevenueRecord(Identification identification, Revenue revenue, BillNumber billNumber, bool isFirstAttempt = true, EvidenceMode mode = EvidenceMode.Online)
        {
            Identifier = Guid.NewGuid();
            Identification = identification;
            Revenue = revenue;
            BillNumber = billNumber;
            IsFirstAttempt = isFirstAttempt;
            Mode = mode;
            Signature = Convert.ToBase64String(GetSignatureBytes());
            SecurityCode = GetSecurityCode();

            //// The generated GUID should comply with UUID v4.0. This is meant to be a sanity check just to make sure the EET specs are correct.
            if (!StringHelpers.SafeMatches(Identifier.ToString(), Patterns.UUID))
            {
                throw new ArgumentException("The generated message UUID does not comply with EET requirements.");
            }
        }

        public Guid Identifier { get; }

        public Identification Identification { get; }

        public Revenue Revenue { get; }

        public BillNumber BillNumber { get; }

        public bool IsFirstAttempt { get; }

        public EvidenceMode Mode { get; }

        public string Signature { get; }

        public string SecurityCode { get; }

        private string GetSecurityCode()
        {
            var hash = new SHA1Managed().ComputeHash(GetSignatureBytes());
            var stringHash = StringHelpers.TransformToBase16(hash);
            return StringHelpers.FormatOctets(stringHash);
        }

        private byte[] GetSignatureBytes()
        {
            var content = $"{Identification.TaxPayerIdentifier.Value}|{Identification.PremisesIdentifier.Value}|{Identification.RegistryIdentifier.Value}|{BillNumber.Value}|{StringHelpers.FormatForEet(Revenue.Accepted)}|{StringHelpers.FormatForEet(Revenue.Gross.Value)}";
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            var formatter = new RSAPKCS1SignatureFormatter(Identification.Certificate.Key);
            formatter.SetHashAlgorithm("SHA256");
            return formatter.CreateSignature(hash);
        }
    }
}
