using System.Linq;
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
            var fixture = Fixtures.Second;

            var certificate = new Certificate(
                password: fixture.CertificatePassword,
                data: fixture.CertificateData
            );
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(fixture.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    premisesIdentifier: new PremisesIdentifier(fixture.PremisesId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m)
                ),
                billNumber: new BillNumber("2016-123")
            );
            var client = new EetClient(certificate, EetEnvironment.Playground);
            var response = client.SendRevenue(record);
            Assert.IsNull(response.Error);
            Assert.IsNotNull(response.Success);
            Assert.IsNotNull(response.Success.FiscalCode);
            Assert.IsFalse(response.Warnings.Any());
        }

        [TestMethod]
        public void SendRevenue()
        {
            var fixture = Fixtures.Third;

            var certificate = new Certificate(
                password: fixture.CertificatePassword,
                data: fixture.CertificateData
            );
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier(fixture.TaxId),
                    registryIdentifier: new RegistryIdentifier("01"),
                    premisesIdentifier: new PremisesIdentifier(fixture.PremisesId),
                    certificate: certificate
                ),
                revenue: new Revenue(
                    gross: new CurrencyValue(1234.00m),
                    notTaxable: new CurrencyValue(0.00m),
                    standardTaxRate: new TaxRateItem(
                        net: new CurrencyValue(100.00m), 
                        tax: new CurrencyValue(21.00m), 
                        goods: null
                    )
                ),
                billNumber: new BillNumber("2016-123")
            );
            var client = new EetClient(certificate, EetEnvironment.Playground);
            var response = client.SendRevenue(record);
            Assert.IsNull(response.Error);
            Assert.IsNotNull(response.Success);
            Assert.IsNotNull(response.Success.FiscalCode);
            Assert.IsFalse(response.Warnings.Any());
        }
    }
}
