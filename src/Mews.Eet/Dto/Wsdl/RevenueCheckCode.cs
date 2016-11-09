using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [Serializable]
    [XmlType(Namespace = "http://fs.mfcr.cz/eet/schema/v3")]
    public class RevenueCheckCode
    {
        [XmlElement(Order = 0, ElementName = "pkp")]
        public SignatureElementType Signature { get; set; }

        [XmlElement(Order = 1, ElementName = "bkp")]
        public SecurityCodeElementType SecurityCode { get; set; }
    }
}
