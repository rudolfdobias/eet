using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        public string SecurityCode
        {
            get
            {
                var hash = new SHA1Managed().ComputeHash(GetSignatureBytes());
                //// Bytes to base16 string.
                var stringHash = String.Concat(hash.Select(b => b.ToString("X2")));
                //// Separate group of 8 characters by a dash. (?!$) is negative lookeahead (last group of 8 is not matched).
                return Regex.Replace(stringHash, ".{8}(?!$)", "$0-");
            }
        }

        private byte[] GetSignatureBytes()
        {
            var content = $"{Identification.TaxPayerIdentifier}|{Identification.PremisesIdentifier}|{Identification.RegistryIdentifier}|{BillNumber}|{Revenue.Accepted}|{Revenue.Gross}";
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            var formatter = new RSAPKCS1SignatureFormatter(Identification.Certificate.Key);
            formatter.SetHashAlgorithm("SHA256");
            return formatter.CreateSignature(hash);
        }
    }
}
