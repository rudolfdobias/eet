using FiscalMachines.CZ.DTO.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiscalMachines.CZ.Tests.IntegrationTests
{
    [TestClass]
    public class Basics
    {
        [TestMethod]
        public void SendRevenue()
        {
            var client = new Client();
            var revenue = new Revenue();
            var payer = new TaxPayer("TaxId");
            var taxIdentification = new TaxIdentification(payer, "register a", premisesIdentifier: 1);
            var response = client.SendRevenue(revenue, taxIdentification);
        }
    }
}
