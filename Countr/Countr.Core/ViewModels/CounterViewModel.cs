using Countr.Core.Models;
using MvvmCross.Core.ViewModels;

namespace Countr.Core.ViewModels
{
    public class CounterViewModel
        : MvxViewModel<Counter>
    {
        private Counter counter;

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
