using System.Threading.Tasks;
using Mews.Eet.Communication;
using Mews.Eet.Dto;

namespace Mews.Eet
{
    public class EetClient
    {
        public EetClient(Certificate certificate, EetEnvironment environment = EetEnvironment.Production, ILogger logger = null)
        {
            EetSoapClient = new EetSoapClient(certificate, environment);
            Logger = logger;
        }

        private EetSoapClient EetSoapClient { get; }

        private ILogger Logger { get; }

        public async Task<SendRevenueResult> SendRevenueAsync(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            Logger?.Info($"Sending bill {record.BillNumber} to EET servers in {mode} mode.");

            var xmlMessage = new SendRevenueMessage(record, mode).GetXmlMessage();
            var sendRevenueResult = await EetSoapClient.SendRevenueAsync(xmlMessage).ConfigureAwait(continueOnCapturedContext: false);
            var result = new SendRevenueResult(sendRevenueResult);

            if (result.IsError)
            {
                Logger?.Info($"Got error response from EET servers for bill {record.BillNumber}.");
            }
            else
            {
                Logger?.Info($"Got success response from EET servers for bill {record.BillNumber}.");
            }

            return result;
        }
    }
}
