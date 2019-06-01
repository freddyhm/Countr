using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Countr.Core.Services;
using MvvmCross.Core.ViewModels;

namespace Countr.Core.ViewModels
{
    public class CountersViewModel : MvxViewModel
    {
        private readonly ICountersService service;
        public ObservableCollection<CounterViewModel> Counters { get; }

        public CountersViewModel(ICountersService service)
        {
            this.service = service;
            Counters = new ObservableCollection<CounterViewModel>();
        }

        public override async Task Initialize()
        {
            await LoadCounters();
        }

        public async Task LoadCounters()
        {
            var counters = await service.GetAllCounters();
            foreach (var counter in counters)
            {
                var viewModel = new CounterViewModel();
                viewModel.Prepare(counter);
                Counters.Add(viewModel);
            }
        }
    }
}
