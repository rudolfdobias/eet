using MewsEet.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MewsEet.Tests.IntegrationTests
{
    [TestClass]
    public class Basics
    {
        private const string GfrTaxId = "CZ72080043";
        private const int PremisesId = 1;

        [TestMethod]
        public void SendRevenueSimple()
        {
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: GfrTaxId,
                    registryIdentifier: "01",
                    premisesIdentifier: PremisesId,
                    key: null /* read pkcs12 cert file */
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
