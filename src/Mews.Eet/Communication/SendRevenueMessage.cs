using Mews.Eet.Converters;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Wsdl;
using Mews.Eet.Extensions;

namespace Mews.Eet.Communication
{
    public class SendRevenueMessage
    {
        public SendRevenueMessage(RevenueRecord record, EetMode mode)
        {
            RevenueRecord = record;
            EetMode = mode;
        }

        private RevenueRecord RevenueRecord { get; }

        private EetMode EetMode { get; }

        public SendRevenueXmlMessage GetXmlMessage()
        {
            var header = new RevenueHeader
            {
                MessageUuid = RevenueRecord.Identifier.ToString(),
                Sent = DateTimeConverter.ToEetDateTime(DateTimeProvider.Now),
                FirstTry = RevenueRecord.IsFirstAttempt,
                Verification = EetMode == EetMode.Verification,
                VerificationSpecified = EetMode == EetMode.Verification
            };

            var revenue = RevenueRecord.Revenue;

            var data = new RevenueData
            {
                TaxPayerTaxIdentifier = RevenueRecord.Identification.TaxPayerIdentifier.Value,
                MandantingTaxPayerIdentifier = RevenueRecord.Identification.MandantingTaxPayerIdentifier?.Value,
                RegistryIdentifier = RevenueRecord.Identification.RegistryIdentifier.Value,
                PremisesIdentifier = RevenueRecord.Identification.PremisesIdentifier.Value,
                BillNumber = RevenueRecord.BillNumber.Value,
                Accepted = DateTimeConverter.ToEetDateTime(RevenueRecord.Revenue.Accepted),
                Total = RevenueRecord.Revenue.Gross.Value,

                NotTaxableNetSpecified = revenue.NotTaxable.IsDefined(),
                NotTaxableNet = revenue.NotTaxable.GetOrDefault(),

                LowerRateNetSpecified = revenue.LowerTaxRate.IsValueDefined(r => r.Net),
                LowerRateTaxSpecified = revenue.LowerTaxRate.IsValueDefined(r => r.Tax),
                LowerRateNet = revenue.LowerTaxRate.GetOrDefault(r => r.Net),
                LowerRateTax = revenue.LowerTaxRate.GetOrDefault(r => r.Tax),

                ReducedRateNetSpecified = revenue.ReducedTaxRate.IsValueDefined(r => r.Net),
                ReducedRateTaxSpecified = revenue.ReducedTaxRate.IsValueDefined(r => r.Tax),
                ReducedRateNet = revenue.ReducedTaxRate.GetOrDefault(r => r.Net),
                ReducedRateTax = revenue.ReducedTaxRate.GetOrDefault(r => r.Tax),

                StandartRateNetSpecified = revenue.StandardTaxRate.IsValueDefined(r => r.Net),
                StandartRateTaxSpecified = revenue.StandardTaxRate.IsValueDefined(r => r.Tax),
                StandartRateNet = revenue.StandardTaxRate.GetOrDefault(r => r.Net),
                StandartRateTax = revenue.StandardTaxRate.GetOrDefault(r => r.Tax),

                TravelServicesSpecified = revenue.TravelServices.IsDefined(),
                TravelServices = revenue.TravelServices.GetOrDefault(),

                LowerRateGoodsSpecified = revenue.LowerTaxRate.IsValueDefined(r => r.Goods),
                LowerRateGoods = revenue.LowerTaxRate.GetOrDefault(r => r.Goods),

                ReducedRateGoodsSpecified = revenue.ReducedTaxRate.IsValueDefined(r => r.Goods),
                ReducedRateGoods = revenue.ReducedTaxRate.GetOrDefault(r => r.Goods),

                StandartRateGoodsSpecified = revenue.StandardTaxRate.IsValueDefined(r => r.Goods),
                StandartRateGoods = revenue.StandardTaxRate.GetOrDefault(r => r.Goods)
            };

            var checkCodes = new RevenueCheckCode()
            {
                SecurityCode = new SecurityCodeElementType
                {
                    Digest = SecurityCodeDigestType.Sha1,
                    Encoding = SecurityCodeEncodingType.Base16,
                    Text = new[] { RevenueRecord.SecurityCode }
                },
                Signature = new SignatureElementType
                {
                    Cipher = SignatureCipherType.Rsa2048,
                    Digest = SignatureDigestType.Sha256,
                    Encoding = SignatureEncodingType.Base64,
                    Text = new[] { RevenueRecord.Signature }
                }
            };
            return new SendRevenueXmlMessage
            {
                Header = header,
                Data = data,
                CheckCodes = checkCodes
            };
        }
    }
}
