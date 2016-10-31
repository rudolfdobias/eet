using System.Threading.Tasks;

namespace Mews.Eet.Soap.Client
{
    public class SoapWsSecurityClient
    {
        public SoapWsSecurityClient(string endpointUrl)
        {
        }

        public Task<O> Post<I, O>(I messageIn, string url)
        {
        }
    }
}
