using System.Threading.Tasks;
using Mews.Eet.Dto;
using Mews.Eet.Messages;
using Mews.Eet.Soap.Client;

namespace Mews.Eet
{
    public class EetClient
    {
        public EetClient(EetEnvironment environment = EetEnvironment.Production)
        {
            Environment = environment;
            SoapClient = new SoapWsSecurityClient(endpointUrl: GetEnvironmentUrl(environment));
        }

        private SoapWsSecurityClient SoapClient { get; }

        private EetEnvironment Environment { get; }

        public SendRevenueResult SendRevenue(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var task = SendRevenueAsync(record, mode);
            task.Wait();
            return task.Result;
        }

        public Task<SendRevenueResult> SendRevenueAsync(RevenueRecord record, EetMode mode = EetMode.Operational)
        {
            var request = new SendRevenueMessage(record, mode).GetRequest();
            var taskCompletionSource = new TaskCompletionSource<SendRevenueResult>();
            SoapClient.Post(request, RevenueUrl).ContinueWith(t =>
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

        private static string GetEnvironmentUrl(EetEnvironment eetEnvironment)
        {
            return "https://pg.eet.cz:443/eet/services/EETServiceSOAP/v3";
        }
    }
}
