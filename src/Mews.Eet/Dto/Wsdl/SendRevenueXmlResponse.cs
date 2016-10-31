using System.ServiceModel;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [MessageContract(WrapperName = "Odpoved", WrapperNamespace = "http://fs.mfcr.cz/eet/schema/v3", IsWrapped = true)]
    public class SendRevenueXmlResponse
    {
        [MessageBodyMember(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 0, Name = "Hlavicka")]
        public ResponseHeader Header;

        [MessageBodyMember(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 1)]
        [XmlElement("Chyba", typeof(ResponseError))]
        [XmlElement("Potvrzeni", typeof(ResponseSuccess))]
        public object Item;

        [MessageBodyMember(Namespace = "http://fs.mfcr.cz/eet/schema/v3", Order = 2)]
        [XmlElement("Varovani")]
        public ResponseWarning[] Warning;

        public SendRevenueXmlResponse(ResponseHeader header, object item, ResponseWarning[] warnings)
        {
            Header = header;
            Item = item;
            Warning = warnings;
        }
    }
}
