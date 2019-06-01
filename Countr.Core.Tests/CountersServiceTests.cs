using System.Collections.Generic;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using Countr.Core.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Countr.Core.Tests
{
    [TestClass]
    public class CountersServiceTests
    {
        private ICountersService service;
        private Mock<ICountersRepository> repo;

        [TestInitialize]
        public void MyTestInitialize()
        {
            repo = new Mock<ICountersRepository>();
            service = new CounterService(repo.Object);
        }

        [TestMethod]
        public async Task IncrementCounter_IncrementsTheCounter()
        {
            // Arrange
            var counter = new Counter {Count = 0};

            // Act
            await service.IncrementCounter(counter);

            // Assert
            Assert.AreEqual(1, counter.Count);
        }

        [TestMethod]
        public async Task IncrementCounter_SavesTheIncrementedCounter()
        {
            // Arrange
            var counter = new Counter {Count = 0};

            // Act
            await service.IncrementCounter(counter);

            // Assert
            repo.Verify(r => r.Save(It.Is<Counter>(c=>c.Count == 1)),
                Times.Once());
        }

        [TestMethod]
        public async Task GetAllCounters_ReturnsAllCountersFromTheRepository()
        {
            // Arrange
            var counters = new List<Counter>
            {
                new Counter() {Name = "Counter1"},
                new Counter() {Name = "Counter2"}
            };

            repo.Setup(r => r.GetAll()).ReturnsAsync(counters);

            // Act
            var results = await service.GetAllCounters();

            // Assert
            CollectionAssert.AreEqual(results, counters);
        }
    }
}
