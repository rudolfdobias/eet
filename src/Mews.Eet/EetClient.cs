using System;
using System.Threading.Tasks;
using Mews.Eet.Communication;
using Mews.Eet.Dto;

namespace Mews.Eet
{
    public class EetClient
    {
        public EetClient(Certificate certificate, EetEnvironment environment = EetEnvironment.Production, TimeSpan? httpTimeout = null, EetLogger logger = null)
        {
            var effectiveTimeout = httpTimeout ?? TimeSpan.FromSeconds(2);
            EetSoapClient = new EetSoapClient(certificate, environment, effectiveTimeout, logger);
            Logger = logger;
        }

        private EetSoapClient EetSoapClient { get; }

        private EetLogger Logger { get; }

        public async Task<SendRevenueResult> SendRevenueAsync(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            Logger?.Info($"Sending revenue record for bill '{record.BillNumber}' to EET servers in {mode} mode.", "Record identifier: " + record.Identifier);

            var xmlMessage = new SendRevenueMessage(record, mode).GetXmlMessage();
            Logger?.Debug($"DTOs for XML mapping were created.", xmlMessage);

            var sendRevenueResult = await EetSoapClient.SendRevenueAsync(xmlMessage).ConfigureAwait(continueOnCapturedContext: false);
            Logger?.Debug($"Result received and successfully deserialized from XML DTOs.", sendRevenueResult);

            var result = new SendRevenueResult(sendRevenueResult);

            if (result.IsError)
            {
                Logger?.Info($"Got error response from EET servers for bill '{record.BillNumber}'.", result);
            }
            else
            {
                Logger?.Info($"Got success response from EET servers for bill '{record.BillNumber}'. FIK: '{result.Success.FiscalCode}'.", result);
            }

            return result;
        }
    }
}
