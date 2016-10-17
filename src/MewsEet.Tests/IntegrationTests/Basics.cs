using MewsEet.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MewsEet.Tests.IntegrationTests
{
    [TestClass]
    public class Basics
    {
        [TestMethod]
        public void SendRevenueSimple()
        {
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: Fixtures.First.TaxId,
                    registryIdentifier: "01",
                    premisesIdentifier: Fixtures.First.PremisesId,
                    certificate: new Certificate(
                        password: Fixtures.First.CertificatePassword,
                        data: Fixtures.First.CertificateData
                    )
                ),     
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: "2016-123"
            );
            var client = new EetClient();
            var response = client.SendRevenue(record);
        }
    }
}
