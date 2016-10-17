using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MewsEet.Dto
{
    public class RevenueRecord
    {
        public RevenueRecord(Identification identification, Revenue revenue, string billNumber, bool isFirstAttempt = true, EvidenceMode mode = EvidenceMode.Online)
        {
            if (!billNumber.IsBillNumber())
            {
                throw new ArgumentException($"A bill number has to match the pattern '{Patterns.BillNumber}'.");
            }

            Identifier = Guid.NewGuid();
            Identification = identification;
            Revenue = revenue;
            BillNumber = billNumber;
            IsFirstAttempt = isFirstAttempt;
            Mode = mode;

            //// The generated GUID should comply with UUID v4.0. This is meant to be a sanity check just to make sure the EET specs are correct.
            if (!Identifier.ToString().IsUUID())
            {
                throw new ArgumentException("The generated message UUID does not comply with EET requirements.");
            }
        }

        public Guid Identifier { get; }

        public Identification Identification { get; }

        public Revenue Revenue { get; }

        public string BillNumber { get; }

        public bool IsFirstAttempt { get; }

        public EvidenceMode Mode { get; }

        public string Signature
        {
            get { return Convert.ToBase64String(GetSignatureBytes()); }
        }

        public string SecurityCode
        {
            get
            {
                var hash = new SHA1Managed().ComputeHash(GetSignatureBytes());
                //// Bytes to base16 string.
                var stringHash = string.Concat(hash.Select(b => b.ToString("X2")));
                //// Separate group of 8 characters by a dash. (?!$) is negative lookeahead (last group of 8 is not matched).
                return Regex.Replace(stringHash, ".{8}(?!$)", "$0-");
            }
        }

        private byte[] GetSignatureBytes()
        {
            var content = $"{Identification.TaxPayerIdentifier}|{Identification.PremisesIdentifier}|{Identification.RegistryIdentifier}|{BillNumber}|{Revenue.Accepted}|{Revenue.Gross}";
            var hash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(content));
            var formatter = new RSAPKCS1SignatureFormatter(Identification.Key);
            formatter.SetHashAlgorithm("SHA256");
            return formatter.CreateSignature(hash);
        }
    }
}
