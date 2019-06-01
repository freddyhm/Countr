using System.Collections.Generic;
using System.Threading.Tasks;
using Countr.Core.Models;
using Countr.Core.Repositories;

namespace Countr.Core.Services
{
    public class CounterService : ICountersService
    {
        private readonly ICountersRepository repository;

        public CounterService(ICountersRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Counter> AddNewCounter(string name)
        {
            var counter = new Counter()
            {
                Name = name
            };

            await repository.Save(counter).ConfigureAwait(false);

            return counter;

        }

        public Task<List<Counter>> GetAllCounters()
        {
            return repository.GetAll();
        }

        public Task DeleteCounter(Counter counter)
        {
            return repository.Delete(counter);
        }

        public Task IncrementCounter(Counter counter)
        {
            counter.Count += 1;
            return repository.Save(counter);
        }
    }
}
