using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using Countr.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Core.Navigation;

namespace Countr.Core.Tests.ViewModels
{
    [TestClass]
    public class CounterViewModelTests
    {
        private Mock<IMvxNavigationService> navigationService;
        private CounterViewModel viewModel;
        private Mock<ICountersService> countersService;

        [TestInitialize]
        public void MyTestInitialize()
        {
            navigationService = new Mock<IMvxNavigationService>();
            countersService = new Mock<ICountersService>();
            viewModel = new CounterViewModel(countersService.Object, navigationService.Object);
            viewModel.ShouldAlwaysRaiseInpcOnUserInterfaceThread(false);
        }

        [TestMethod]
        public async Task SaveCommand_SavesTheCounter()
        {
            // Arrange
            var counter = new Counter { Name = "A Counter" };
            viewModel.Prepare(counter);

            // Act
            await viewModel.SaveCommand.ExecuteAsync();

            // Assert
            countersService.Verify(c => c.AddNewCounter("A Counter"));
            navigationService.Verify(n => n.Close(viewModel));
        }

        [TestMethod]
        public void CancelCommand_DoesntSaveTheCounter()
        {
            // Arrange
            var counter = new Counter { Name = "A Counter" };
            viewModel.Prepare(counter);

            // Act
            viewModel.CancelCommand.Execute();

            // Assert
            countersService.Verify(c => c.AddNewCounter(It.IsAny<string>()),
                Times.Never());
            navigationService.Verify(n => n.Close(viewModel));
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

        [TestMethod]
        public async Task IncrementCounter_IncrementsTheCounter()
        {
            // Act
            await viewModel.IncrementCommand.ExecuteAsync();

            // Assert
            countersService.Verify(s => s.IncrementCounter(It.IsAny<Counter>()));
        }

        [TestMethod]
        public async Task IncrementCounter_RaisesPropertyChanged()
        {
            // Arrange
            var propertyChangeRaised = false;
            viewModel.PropertyChanged +=
                (s, e) => propertyChangeRaised = (e.PropertyName == "Count");

            // Act
            await viewModel.IncrementCommand.ExecuteAsync();

            // Assert
            Assert.IsTrue(propertyChangeRaised);
        }


    }
}
