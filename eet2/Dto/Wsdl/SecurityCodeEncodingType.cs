using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [XmlType(Namespace = "http://fs.mfcr.cz/eet/schema/v3")]
    public enum SecurityCodeEncodingType
    {
        [XmlEnum("base16")]
        Base16
    }
}
