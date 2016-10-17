using System.Threading.Tasks;
using MewsEet.Dto;
using MewsEet.EetService;

namespace MewsEet
{
    public class EetClient
    {
        private EetEnvironment Environment { get; }

        public EetClient(EetEnvironment environment = EetEnvironment.Production)
        {
            Environment = environment;
        }

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
                } else if (t.IsCanceled)
                {
                    taskCompletionSource.TrySetCanceled();
                }
                else
                {
                    taskCompletionSource.TrySetResult(SendRevenueResult.Create(t.Result));
                }
            });
            return taskCompletionSource.Task;
        }

        private OdeslaniTrzbyRequest GetRevenueRequest(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var header = new TrzbaHlavickaType
            {
                uuid_zpravy = record.Identifier.ToString(),
                dat_odesl = DateTimeProvider.Now.EetDateTime,
                prvni_zaslani = record.IsFirstAttempt,
                overeni = mode == EetMode.Verification,
                overeniSpecified = mode == EetMode.Verification
            };

            var revenue = record.Revenue;

            var data = new TrzbaDataType
            {
                dic_popl = record.Identification.TaxPayerIdentifier,
                dic_poverujiciho = record.Identification.MandantingTaxPayerIdentifier,
                id_pokl = record.Identification.RegistryIdentifier,
                id_provoz = record.Identification.PremisesIdentifier,
                porad_cis = record.BillNumber,
                dat_trzby = record.Revenue.Accepted.EetDateTime,
                celk_trzba = record.Revenue.Gross.Value,

                zakl_nepodl_dphSpecified = revenue.NotTaxable.IsDefined(),
                zakl_nepodl_dph = revenue.NotTaxable.SafeValue(),

                zakl_dan1Specified = revenue.LowerTaxRate.IsDefined(r => r.Net),
                dan1Specified = revenue.LowerTaxRate.IsDefined(r => r.Tax),
                zakl_dan1 = revenue.LowerTaxRate.SafeValue(r => r.Net),
                dan1 = revenue.LowerTaxRate.SafeValue(r => r.Tax),

                zakl_dan2Specified = revenue.ReducedTaxRate.IsDefined(r => r.Net),
                dan2Specified = revenue.ReducedTaxRate.IsDefined(r => r.Tax),
                zakl_dan2 = revenue.ReducedTaxRate.SafeValue(r => r.Net),
                dan2 = revenue.ReducedTaxRate.SafeValue(r => r.Tax),

                zakl_dan3Specified = revenue.StandardTaxRate.IsDefined(r => r.Net),
                dan3Specified = revenue.StandardTaxRate.IsDefined(r => r.Tax),
                zakl_dan3 = revenue.StandardTaxRate.SafeValue(r => r.Net),
                dan3 = revenue.StandardTaxRate.SafeValue(r => r.Tax),
                
                cest_sluzSpecified = revenue.TravelServices.IsDefined(),
                cest_sluz = revenue.TravelServices.SafeValue(),

                pouzit_zboz1Specified = revenue.LowerTaxRate.IsDefined(r => r.Goods),
                pouzit_zboz1 = revenue.LowerTaxRate.SafeValue(r => r.Goods),

                pouzit_zboz2Specified = revenue.ReducedTaxRate.IsDefined(r => r.Goods),
                pouzit_zboz2 = revenue.ReducedTaxRate.SafeValue(r => r.Goods),

                pouzit_zboz3Specified = revenue.StandardTaxRate.IsDefined(r => r.Goods),
                pouzit_zboz3 = revenue.StandardTaxRate.SafeValue(r => r.Goods)
            };

            var checkCodes = new TrzbaKontrolniKodyType();
            
            return new OdeslaniTrzbyRequest(header, data, checkCodes);
        }
    }
}
