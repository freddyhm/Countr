using Countr.Core.Models;
using Countr.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Countr.Core.Tests.ViewModels
{
    [TestClass]
    public class CounterViewModelTests
    {
        private CounterViewModel viewModel;

        [TestInitialize]
        public void MyTestInitialize()
        {
            viewModel = new CounterViewModel();
        }

        [TestMethod]
        public void Name_ComesFromCounter()
        {
            // Arrange
            var counter = new Counter() { Name = "A Counter"};

            // Act
            viewModel.Prepare(counter);

            // Assert
            Assert.AreEqual(counter.Name, viewModel.Name);
        }
    }
}
