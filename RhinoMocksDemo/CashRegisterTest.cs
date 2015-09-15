using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace RhinoMocksDemo
{
    [TestClass]
    public class CashRegisterTest
    {
        private CashRegister _cashRegister;

        /// <summary>
        /// This setup is called before each test is executed
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _cashRegister = new CashRegister();
        }

        [TestMethod]
        public void CalculateTotal_OneLineItem_ReturnsPrice()
        {
            //  Arrange
            _cashRegister.AddItem("Bread", 2.00);

            //  Act
            var total = _cashRegister.CalculateTotal();

            //  Assert
            Assert.AreEqual(2.00, total);
        }

        [TestMethod]
        public void CalculateTotal_ManyLineItems_ReturnsPrice()
        {
            //  Arrange
            _cashRegister.AddItem("Bread", 2.00);
            _cashRegister.AddItem("Milk", 2.50);
            _cashRegister.AddItem("Bannanas", 1.75);

            //  Act
            var total = _cashRegister.CalculateTotal();

            //  Assert
            Assert.AreEqual(6.25, total);
        }

        [TestMethod]
        public void CompleteSale_ManyLineItems_ReturnsCorrectChange()
        {
            //  Arrange
            _cashRegister.AddItem("Bread", 2.00);
            _cashRegister.AddItem("Milk", 2.50);
            _cashRegister.AddItem("Bannanas", 1.75);

            //  Act
            var change = _cashRegister.CompleteSale(7.00);

            //  Assert
            Assert.AreEqual(0.75, change);
        }
    }
}
