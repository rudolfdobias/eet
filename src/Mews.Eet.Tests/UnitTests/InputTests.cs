using System;
using Mews.Eet.Dto.Identifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mews.Eet.Tests.UnitTests
{
    [TestClass]
    public class InputTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TaxIdValidationWorks()
        {
            new TaxIdentifier("abcd");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RegistryIdValidationWorks()
        {
            new RegistryIdentifier(Guid.NewGuid().ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BillNumberValidationWorks()
        {
            new BillNumber(Guid.NewGuid().ToString());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PremisesIdValidationWorks()
        {
            new PremisesIdentifier(0);
        }

        [TestMethod]
        public void TaxIdWorks()
        {
            Assert.IsNotNull(new TaxIdentifier("CZ12345678"));
        }

        [TestMethod]
        public void RegistryIdWorks()
        {
            Assert.IsNotNull(new RegistryIdentifier("1"));
        }


        [TestMethod]
        public void BillNumberWorks()
        {
            Assert.IsNotNull(new BillNumber("2016020004"));
        }


        [TestMethod]
        public void PremisesIdWorks()
        {
            Assert.IsNotNull(new PremisesIdentifier(1));
        }
    }
}
