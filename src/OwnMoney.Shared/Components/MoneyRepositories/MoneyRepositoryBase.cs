using OwnMoney.Shared.Domains.RepositorySubscription;
using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using OwnMoney.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Shared.Components.MoneyRepositories
{
    public abstract class MoneyRepositoryBase : IMoneyRepository
    {
        protected readonly ISubscriptionTracker _subscriptionTracker;

        public MoneyRepositoryBase(ISubscriptionTracker subscriptionTracker)
        {
            _subscriptionTracker = subscriptionTracker;
        }

        public abstract Task<IEnumerable<Entry>> GetEntries(GetEntriesQuery query);

        public Task<SubscriptionResponse> SubscribeToChanges(SubscriptionRequest request) => _subscriptionTracker.TrackSubscriber(request);

        public void UnsubscribeFromChanges(Guid requestId) => _subscriptionTracker.UntrackSubscriber(requestId);

        public async Task UpsertEntries(IEnumerable<Entry> entries)
        {
            await UpsertEntriesIntoStorage(entries);
            await _subscriptionTracker.TrackChanges(entries, new long[0]);
        }

        ///<summary> Perform the upsert into the storage implementation </summary>
        protected abstract Task UpsertEntriesIntoStorage(IEnumerable<Entry> entries);

        public async Task DeleteEntries(IEnumerable<long> entries)
        {
            await DeleteEntriesFromStorage(entries);
            await _subscriptionTracker.TrackChanges(new Entry[0], entries);
        }

        ///<summary> Perform the deletion from the storage implementation </summary>
        public abstract Task DeleteEntriesFromStorage(IEnumerable<long> entries);
    }
}
