using Mews.Eet.Dto;
using Mews.Eet.Dto.Identifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mews.Eet.Tests.IntegrationTests
{
    [TestClass]
    public class Basics
    {
        [TestMethod]
        public void SendRevenueSimple()
        {
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(Fixtures.First.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    premisesIdentifier: new PremisesIdentifier(Fixtures.First.PremisesId),
                    certificate: new Certificate(
                        password: Fixtures.First.CertificatePassword,
                        data: Fixtures.First.CertificateData
                    )
                ),     
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: new BillNumber("2016-123")
            );
            var client = new EetClient();
            var response = client.SendRevenue(record);
            Assert.IsNull(response.Error);
            Assert.IsNotNull(response.Success);
        }
    }
}
