using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Services;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace Countr.Core.ViewModels
{
    public class CounterViewModel
        : MvxViewModel<Counter>
    {
        private IMvxNavigationService navigationService;
        private readonly ICountersService service;
        private Counter counter;
        public IMvxAsyncCommand IncrementCommand { get; }
        public IMvxAsyncCommand DeleteCommand { get; }
        public IMvxAsyncCommand CancelCommand { get; }
        public IMvxAsyncCommand SaveCommand { get; }

        public CounterViewModel(ICountersService service, 
            IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.service = service;
            IncrementCommand = new MvxAsyncCommand(IncrementCounter);
            DeleteCommand = new MvxAsyncCommand(DeleteCounter);
            SaveCommand = new MvxAsyncCommand(Save);
            CancelCommand = new MvxAsyncCommand(Cancel);
        }

        async Task Cancel()
        {
            await navigationService.Close(this);
        }

        async Task Save()
        {
            await service.AddNewCounter(counter.Name);
            await navigationService.Close(this);
        }

        async Task IncrementCounter()
        {
            await service.IncrementCounter(counter);
            RaisePropertyChanged(() => Count);
        }

        async Task DeleteCounter()
        {
            await service.DeleteCounter(counter);
        }

        public override void Prepare(Counter counter)
        {
            this.counter = counter;
        }

        public string Name
        {
            get { return counter.Name; }

            set
            {
                if (Name == value)
                    return;

                counter.Name = value;
                RaisePropertyChanged();
            }
        }

        public int Count => counter.Count;




    }
}
