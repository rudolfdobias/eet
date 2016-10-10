using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using FiscalMachines.CZ.DTO.Data;
using FiscalMachines.CZ.DTO.Service;
using FiscalMachines.CZ.EETService;
using FuncSharp;

namespace FiscalMachines.CZ
{
    public class Client
    {
        private EETEnvironment Environment { get; }

        public Client(EETEnvironment environment = EETEnvironment.Production)
        {
            Environment = environment;
        }

        public EETSendRevenueResult SendRevenue(Revenue revenue, TaxIdentification taxIdentification, EETMode mode = EETMode.Operational)
        {
            var task = SendRevenueAsync(revenue, taxIdentification, mode);
            task.Wait();
            return task.Result;
        }

        public Task<EETSendRevenueResult> SendRevenueAsync(Revenue revenue, TaxIdentification taxIdentification, EETMode mode = EETMode.Operational)
        {
            var client = EETClients.Create(Environment);
            var request = GetRevenueRequest(revenue, taxIdentification, mode);
            var taskCompletionSource = new TaskCompletionSource<EETSendRevenueResult>();
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
                    taskCompletionSource.TrySetResult(ProcessRevenueResponse(t.Result));
                }
            });
            return taskCompletionSource.Task;
        }

        private OdeslaniTrzbyRequest GetRevenueRequest(Revenue revenue, TaxIdentification taxIdentification, EETMode mode = EETMode.Operational)
        {
            var header = new TrzbaHlavickaType
            {
                uuid_zpravy = revenue.Identifier.ToString(),
                dat_odesl = DateTime.Now,
                prvni_zaslani = true, // TODO
                overeni = mode == EETMode.Verification,
                overeniSpecified = true
            };
            
            // TODO consider extracting methods to taxPayer
            var payer = taxIdentification.Payer;
            var reporterTaxIdentifier = payer.Representation == null || payer.Representation.Type != TaxPayerRepresentationType.IndirectRepresentation ? payer.TaxIdentifier : payer.Representation.RepresentingTaxPayer.TaxIdentifier;
            var representativeTaxIdentifier = payer.Representation != null && payer.Representation.Type == TaxPayerRepresentationType.AuthorizedTransferredRepresentation ? payer.TaxIdentifier : null;

            var data = new TrzbaDataType
            {
                dic_popl = reporterTaxIdentifier,
                dic_poverujiciho = representativeTaxIdentifier,
                id_pokl = taxIdentification.RegisterIdentifier,
                id_provoz = taxIdentification.PremisesIdentifier,
                porad_cis = revenue.BillNumber,
                dat_trzby = revenue.Created,
                celk_trzba = revenue.Total,
                zakl_nepodl_dphSpecified = revenue.NotTaxableNet.HasValue,
                zakl_nepodl_dph = Value(revenue.NotTaxableNet),

                zakl_dan1Specified = revenue.LowerReducedTaxItem != null,
                dan1Specified = revenue.LowerReducedTaxItem != null,
                zakl_dan1 = revenue.LowerReducedTaxItem?.Net ?? default(decimal),
                dan1 = revenue.LowerReducedTaxItem?.Tax ?? default(decimal),

                zakl_dan2Specified = revenue.ReducedTaxItem != null,
                dan2Specified = revenue.ReducedTaxItem != null,
                zakl_dan2 = revenue.ReducedTaxItem?.Net ?? default(decimal),
                dan2 = revenue.ReducedTaxItem?.Tax ?? default(decimal),

                zakl_dan3Specified = revenue.StandardTaxItem != null,
                dan3Specified = revenue.StandardTaxItem != null,
                zakl_dan3 = revenue.StandardTaxItem?.Net ?? default(decimal),
                dan3 = revenue.StandardTaxItem?.Tax ?? default(decimal),

            };

            var checkCodes = new TrzbaKontrolniKodyType();

            return new OdeslaniTrzbyRequest(header, data, checkCodes);
        }

        private decimal Value(decimal? value)
        {
            return value ?? default(decimal);
        }

        private EETSendRevenueResult ProcessRevenueResponse(OdeslaniTrzbyResponse response)
        {
            var header = response.Hlavicka;
            var item = response.Item;
            var warnings = response.Varovani;
            return new EETSendRevenueResult();
        }
    }
}
