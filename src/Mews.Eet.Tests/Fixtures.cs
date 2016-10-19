namespace Mews.Eet.Tests
{
    public class Fixtures
    {
        public static TaxPayerFixture First = new TaxPayerFixture
        {
            TaxId = "CZ1212121218",
            PremisesId = 1,
            CertificatePassword = "eet"
        };

        public static TaxPayerFixture Second = new TaxPayerFixture
        {
            TaxId = "CZ00000019",
            PremisesId = 1,
            CertificatePassword = "eet"
        };

        public static TaxPayerFixture Third = new TaxPayerFixture
        {
            TaxId = "CZ1212121218",
            PremisesId = 1,
            CertificatePassword = "eet"
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
