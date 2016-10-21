using System.IO;

namespace Mews.Eet.Tests
{
    public class Fixtures
    {
        public static TaxPayerFixture First = new TaxPayerFixture
        {
            TaxId = "CZ1212121218",
            PremisesId = 1,
            CertificatePassword = "eet",
            CertificateData = File.ReadAllBytes("Certificates/01000003.p12")
        };

        public static TaxPayerFixture Second = new TaxPayerFixture
        {
            TaxId = "CZ00000019",
            PremisesId = 1,
            CertificatePassword = "eet",
            CertificateData = File.ReadAllBytes("Certificates/01000004.p12")
        };

        public static TaxPayerFixture Third = new TaxPayerFixture
        {
            TaxId = "CZ1212121218",
            PremisesId = 1,
            CertificatePassword = "eet",
            CertificateData = File.ReadAllBytes("Certificates/01000005.p12")
        };
    }

    public class TaxPayerFixture
    {
        public string TaxId { get; set; }
        public int PremisesId { get; set; }
        public string CertificatePassword { get; set; }
        public byte[] CertificateData { get; set; }
    }
}
