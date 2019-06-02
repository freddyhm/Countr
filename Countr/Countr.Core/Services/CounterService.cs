using System.Collections.Generic;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Repositories;
using MvvmCross.Plugins.Messenger;

namespace Countr.Core.Services
{
    public class CounterService : ICountersService
    {
        private readonly ICountersRepository repository;
        private readonly IMvxMessenger messenger;

        public CounterService(ICountersRepository repository, IMvxMessenger messenger
        )
        {
            this.repository = repository;
            this.messenger = messenger;
        }

        public async Task<Counter> AddNewCounter(string name)
        {
            var counter = new Counter()
            {
                Name = name
            };

            await repository.Save(counter).ConfigureAwait(false);

            messenger.Publish(new CountersChangedMessage(this));

            return counter;

        }

        public Task<List<Counter>> GetAllCounters()
        {
            return repository.GetAll();
        }

        public async Task DeleteCounter(Counter counter)
        {
            await repository.Delete(counter).ConfigureAwait(false);
            messenger.Publish(new CountersChangedMessage(this));
        }

        public Task IncrementCounter(Counter counter)
        {
            counter.Count += 1;
            return repository.Save(counter);
        }
    }
}
