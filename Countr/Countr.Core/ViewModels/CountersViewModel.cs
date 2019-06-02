using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace Countr.Core.ViewModels
{
    public class CountersViewModel : MvxViewModel
    {
        private readonly MvxSubscriptionToken token;
        private readonly IMvxNavigationService navigationService;
        private readonly ICountersService service;
        public ObservableCollection<CounterViewModel> Counters { get; }
        public IMvxAsyncCommand ShowAddNewCounterCommand { get; }

        public CountersViewModel(ICountersService service, IMvxMessenger messenger, IMvxNavigationService navigationService)
        {
            token = messenger
                .SubscribeOnMainThread<CountersChangedMessage>
                    (async m => await LoadCounters());

            this.service = service;
            this.navigationService = navigationService;
            ShowAddNewCounterCommand = new MvxAsyncCommand(ShowAddNewCounter);
            Counters = new ObservableCollection<CounterViewModel>();
        }

        public override async Task Initialize()
        {
            await LoadCounters();
        }

        async Task ShowAddNewCounter()
        {
            await navigationService.Navigate(typeof(CounterViewModel), new Counter());
        }

        public async Task LoadCounters()
        {
            Counters.Clear();
            var counters = await service.GetAllCounters();
            foreach (var counter in counters)
            {
                var viewModel = new CounterViewModel(service, navigationService);
                viewModel.Prepare(counter);
                Counters.Add(viewModel);
            }
        }
    }
}
