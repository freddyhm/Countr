using System.Collections.Generic;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using Countr.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Countr.Core.Tests.ViewModels
{
    [TestClass]
    public class CountersViewModelTests
    {
        private Mock<ICountersService> countersService;
        private CountersViewModel viewModel;

        [TestInitialize]
        public void MyTestInitialize()
        {
            countersService = new Mock<ICountersService>();
            viewModel = new CountersViewModel(countersService.Object);
        }

        [TestMethod]
        public async Task LoadCounters_CreatesCounters()
        {
            // Arrange
            var counters = new List<Counter>
            {
                new Counter {Name = "Counter1", Count = 0},
                new Counter {Name = "Counter2", Count = 4},
            };

            countersService.Setup(c => c.GetAllCounters())
                .ReturnsAsync(counters);

            // Act
            viewModel.RaisePropertyChanged();
            await viewModel.LoadCounters();

            // Assert
            Assert.AreEqual(2, viewModel.Counters.Count);
            Assert.AreEqual("Counter1", viewModel.Counters[0].Name);
            Assert.AreEqual(0, viewModel.Counters[0].Count);
            Assert.AreEqual("Counter2", viewModel.Counters[1].Name);
            Assert.AreEqual(4, viewModel.Counters[1].Count);
        }
    }
}
