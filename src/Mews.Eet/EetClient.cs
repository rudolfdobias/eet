using System;
using System.Threading.Tasks;
using Mews.Eet.Dto;
using Mews.Eet.EetService;
using Mews.Eet.Extensions;

namespace Mews.Eet
{
    public class EetClient
    {
        public EetClient(EetEnvironment environment = EetEnvironment.Production)
        {
            Environment = environment;
        }

        private EetEnvironment Environment { get; }

        public SendRevenueResult SendRevenue(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var task = SendRevenueAsync(record, mode);
            task.Wait();
            return task.Result;
        }

        public Task<SendRevenueResult> SendRevenueAsync(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var client = EetClients.Create(Environment);
            var request = GetRevenueRequest(record, mode);
            var taskCompletionSource = new TaskCompletionSource<SendRevenueResult>();
            client.OdeslaniTrzbyAsync(request).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    taskCompletionSource.TrySetException(t.Exception);
                }
                else if (t.IsCanceled)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                else
                {
                    taskCompletionSource.TrySetResult(new SendRevenueResult(t.Result));
                }
            });
            return taskCompletionSource.Task;
        }

        private OdeslaniTrzbyRequest GetRevenueRequest(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var header = new TrzbaHlavickaType
            {
                uuid_zpravy = record.Identifier.ToString(),
                dat_odesl = EetDateTime(DateTimeProvider.Now),
                prvni_zaslani = record.IsFirstAttempt,
                overeni = mode == EetMode.Verification,
                overeniSpecified = mode == EetMode.Verification
            };

            var revenue = record.Revenue;

            var data = new TrzbaDataType
            {
                dic_popl = record.Identification.TaxPayerIdentifier.Value,
                dic_poverujiciho = record.Identification.MandantingTaxPayerIdentifier?.Value,
                id_pokl = record.Identification.RegistryIdentifier.Value,
                id_provoz = record.Identification.PremisesIdentifier.Value,
                porad_cis = record.BillNumber.Value,
                dat_trzby = EetDateTime(record.Revenue.Accepted),
                celk_trzba = record.Revenue.Gross.Value,

                zakl_nepodl_dphSpecified = revenue.NotTaxable.IsDefined(),
                zakl_nepodl_dph = revenue.NotTaxable.GetOrDefault(),

                zakl_dan1Specified = revenue.LowerTaxRate.IsValueDefined(r => r.Net),
                dan1Specified = revenue.LowerTaxRate.IsValueDefined(r => r.Tax),
                zakl_dan1 = revenue.LowerTaxRate.GetOrDefault(r => r.Net),
                dan1 = revenue.LowerTaxRate.GetOrDefault(r => r.Tax),

                zakl_dan2Specified = revenue.ReducedTaxRate.IsValueDefined(r => r.Net),
                dan2Specified = revenue.ReducedTaxRate.IsValueDefined(r => r.Tax),
                zakl_dan2 = revenue.ReducedTaxRate.GetOrDefault(r => r.Net),
                dan2 = revenue.ReducedTaxRate.GetOrDefault(r => r.Tax),

                zakl_dan3Specified = revenue.StandardTaxRate.IsValueDefined(r => r.Net),
                dan3Specified = revenue.StandardTaxRate.IsValueDefined(r => r.Tax),
                zakl_dan3 = revenue.StandardTaxRate.GetOrDefault(r => r.Net),
                dan3 = revenue.StandardTaxRate.GetOrDefault(r => r.Tax),

                cest_sluzSpecified = revenue.TravelServices.IsDefined(),
                cest_sluz = revenue.TravelServices.GetOrDefault(),

                pouzit_zboz1Specified = revenue.LowerTaxRate.IsValueDefined(r => r.Goods),
                pouzit_zboz1 = revenue.LowerTaxRate.GetOrDefault(r => r.Goods),

                pouzit_zboz2Specified = revenue.ReducedTaxRate.IsValueDefined(r => r.Goods),
                pouzit_zboz2 = revenue.ReducedTaxRate.GetOrDefault(r => r.Goods),

                pouzit_zboz3Specified = revenue.StandardTaxRate.IsValueDefined(r => r.Goods),
                pouzit_zboz3 = revenue.StandardTaxRate.GetOrDefault(r => r.Goods)
            };

            var checkCodes = new TrzbaKontrolniKodyType()
            {
                bkp = new BkpElementType
                {
                    digest = BkpDigestType.SHA1,
                    encoding = BkpEncodingType.base16,
                    Text = new[] { record.SecurityCode }
                },
                pkp = new PkpElementType
                {
                    cipher = PkpCipherType.RSA2048,
                    digest = PkpDigestType.SHA256,
                    encoding = PkpEncodingType.base64,
                    Text = new[] { record.Signature }
                }
            };
            return new OdeslaniTrzbyRequest(header, data, checkCodes);
        }

        private DateTime EetDateTime(DateTimeWithTimeZone dateTimeWithTimeZone)
        {
            var dateTimeUtc = TimeZoneInfo.ConvertTimeToUtc(dateTimeWithTimeZone.DateTime, dateTimeWithTimeZone.TimeZoneInfo);
            var czDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, DateTimeWithTimeZone.CzechTimeZone);
            return DateTime.SpecifyKind(new DateTime(czDateTime.Ticks - czDateTime.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Local);
        }
    }
}
