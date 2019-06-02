using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using Countr.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvvmCross.Core.Navigation;
using MvvmCross.Plugins.Messenger;

namespace Countr.Core.Tests.ViewModels
{
    [TestClass]
    public class CountersViewModelTests
    {
        private Mock<IMvxNavigationService> navigationService;
        private Mock<ICountersService> countersService;
        private Mock<IMvxMessenger> messenger;
        private CountersViewModel viewModel;
        private Action<CountersChangedMessage> publishAction;

        [TestInitialize]
        public void MyTestInitialize()
        {
            navigationService = new Mock<IMvxNavigationService>();
            messenger = new Mock<IMvxMessenger>();

            messenger.Setup(m => m.SubscribeOnMainThread
                (It.IsAny<Action<CountersChangedMessage>>(),
                    It.IsAny<MvxReference>(),
                    It.IsAny<string>()))
                .Callback<Action<CountersChangedMessage>,
                    MvxReference, string>((a, m, s) => publishAction = a);

            countersService = new Mock<ICountersService>();
            viewModel = new CountersViewModel(countersService.Object, 
                messenger.Object, navigationService.Object);
        }

        [TestMethod]
        public async Task ShowAddNewCounterCommand_ShowsCounterViewModel()
        {
            // Act
            await viewModel.ShowAddNewCounterCommand.ExecuteAsync();

            // Assert
            navigationService.Verify(n => n.Navigate(typeof(CounterViewModel),
                It.IsAny<Counter>(),
                null));
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

        [TestMethod]
        public void ReceivedMessage_LoadsCounters()
        {
            // Arrange
             countersService.Setup(s => s.GetAllCounters())
                .ReturnsAsync(new List<Counter>());

            // Act
            publishAction.Invoke(new CountersChangedMessage(this));

            // Assert 
            countersService.Verify(s => s.GetAllCounters());
        }
    }
}
