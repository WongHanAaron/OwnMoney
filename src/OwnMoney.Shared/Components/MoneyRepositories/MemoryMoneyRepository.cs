using OwnMoney.Shared.Domains.RepositorySubscription;
using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Shared.Components.MoneyRepositories
{
    public class MemoryMoneyRepository : MoneyRepositoryBase
    {
        public ConcurrentDictionary<long, Entry> _entries = new ConcurrentDictionary<long, Entry>();

        public MemoryMoneyRepository(ISubscriptionTracker subscriptionTracker) : base(subscriptionTracker)
        {
        }

        public override Task DeleteEntriesFromStorage(IEnumerable<long> entries)
        {
            foreach (var entry in entries)
            {
                _entries.TryRemove(entry, out _);
            }
            return Task.CompletedTask;
        }

        public override Task<IEnumerable<Entry>> GetEntries(GetEntriesQuery query)
            => Task.FromResult(_entries.Values.AsQueryable().Filter(query).AsEnumerable());

        protected override Task UpsertEntriesIntoStorage(IEnumerable<Entry> entries)
        {
            foreach (var entry in entries)
            {
                _entries[entry.Id] = entry;
            }
            return Task.CompletedTask;
        }
    }
}
