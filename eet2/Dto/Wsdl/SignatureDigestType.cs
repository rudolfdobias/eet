using System;
using System.Xml.Serialization;

namespace Mews.Eet.Dto.Wsdl
{
    [XmlType(Namespace = "http://fs.mfcr.cz/eet/schema/v3")]
    public enum SignatureDigestType
    {
        [XmlEnum("SHA256")]
        Sha256
    }
}
