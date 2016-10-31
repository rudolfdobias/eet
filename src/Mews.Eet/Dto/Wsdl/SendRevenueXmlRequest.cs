using System.ServiceModel;

namespace Mews.Eet.Dto.Wsdl
{
    [MessageContract(WrapperName = "Trzba", WrapperNamespace = "http://fs.mfcr.cz/eet/schema/v3", IsWrapped = true)]
    public class SendRevenueXmlRequest
    {
        [MessageBodyMemberAttribute(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 0, Name = "Hlavicka")]
        public RevenueHeaderType Header;

        [MessageBodyMemberAttribute(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 1)]
        public RevenueDataType Data;

        [MessageBodyMemberAttribute(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 2, Name = "KontrolniKody")]
        public RevenueCheckCode CheckCodes;

        public SendRevenueXmlRequest(RevenueHeaderType header, RevenueDataType data, RevenueCheckCode checkCodes)
        {
            Header = header;
            Data = data;
            CheckCodes = checkCodes;
        }
    }
}
