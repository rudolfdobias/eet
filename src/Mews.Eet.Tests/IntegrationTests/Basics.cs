using System;
using System.Linq;
using System.Threading.Tasks;
using Mews.Eet.Dto;
using Mews.Eet.Dto.Identifiers;
using Xunit;

namespace Mews.Eet.Tests.IntegrationTests
{
    public class Basics
    {
        [Fact]
        public async Task SendRevenueSimple()
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
            var response = await client.SendRevenueAsync(record);
            Assert.Null(response.Error);
            Assert.NotNull(response.Success);
            Assert.NotNull(response.Success.FiscalCode);
            Assert.False(response.Warnings.Any());
        }

        [Fact]
        public async Task TimeoutWorks()
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
            await Assert.ThrowsAsync<TaskCanceledException>(async () => await client.SendRevenueAsync(record, httpTimeout: TimeSpan.FromMilliseconds(1)));
        }

        [Fact]
        public async Task SendRevenue()
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
            var response = await client.SendRevenueAsync(record);
            Assert.Null(response.Error);
            Assert.NotNull(response.Success);
            Assert.NotNull(response.Success.FiscalCode);
            Assert.False(response.Warnings.Any());
        }

        [Fact]
        public async Task HandlesError()
        {
            var fixture = Fixtures.First;
            var certificate = new Certificate(
                password: fixture.CertificatePassword,
                data: fixture.CertificateData
            );
            var record = new RevenueRecord(
                identification: new Identification(
                    taxPayerIdentifier: new TaxIdentifier("CZ1234567891"),
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
            var response = await client.SendRevenueAsync(record);
            Assert.NotNull(response.Error);
            Assert.Equal(response.Error.Reason.Code, 6);
        }
    }
}
