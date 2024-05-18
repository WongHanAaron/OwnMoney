using LiteDB;
using OwnMoney.Shared.Domains.RepositorySubscription;
using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Shared.Components.MoneyRepositories
{
    public class LiteDbMoneyRepository : MoneyRepositoryBase
    {
        public const string EntryCollectionName = "entry_collection";
        protected readonly LiteDbConfiguration _config;

        public LiteDbMoneyRepository(LiteDbConfiguration config, ISubscriptionTracker subscriptionTracker) : base(subscriptionTracker)
        {
            _config = config;
        }

        public override Task DeleteEntriesFromStorage(IEnumerable<long> entries)
        {
            using (var db = new LiteDatabase(_config.DatabasePath))
            {
                var toDelete = entries.ToLookup(e => e);
                db.GetCollection<Entry>(EntryCollectionName).DeleteMany(d => toDelete.Contains(d.Id));
            }
            return Task.CompletedTask;
        }

        public override Task<IEnumerable<Entry>> GetEntries(GetEntriesQuery query)
        {
            using (var db = new LiteDatabase(_config.DatabasePath))
            {
                var returned = db.GetCollection<Entry>(EntryCollectionName).Find(d => query.MatchesQuery(d));

                return Task.FromResult(returned);
            }
        }

        protected override Task UpsertEntriesIntoStorage(IEnumerable<Entry> entries)
        {
            using (var db = new LiteDatabase(_config.DatabasePath))
            {
                var returned = db.GetCollection<Entry>(EntryCollectionName).Upsert(entries);

                return Task.FromResult(returned);
            }
        }
    }

    public class LiteDbConfiguration
    {
        public string DatabasePath { get; set; }
    }
}
