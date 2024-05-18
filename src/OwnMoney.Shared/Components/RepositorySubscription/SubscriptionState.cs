using OwnMoney.Shared.Models.Requests;
using OwnMoney.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwnMoney.Shared.Components.RepositorySubscription
{
    ///<summary> Stores the state data for the Subscription Tracker </summary>
    public class SubscriptionState
    {
        ///<summary> When this subscriber expires </summary>
        public DateTime ExpiresOn { get; set; }

        ///<summary> When the subscriber was last updated </summary>
        public DateTime LastUpdated { get; set; }

        ///<summary> The request that was made </summary>
        public SubscriptionRequest Request { get; set; }

        ///<summary> The entries that have changed since the last update </summary>
        public HashSet<long> ChangedEntries { get; set; } = new HashSet<long>();

        ///<summary> The entries that have been deleted since the last update </summary>
        public HashSet<long> DeletedEntries { get; set; } = new HashSet<long>();

        public ChangedContents FlushChanges()
        {
            var returned = new ChangedContents()
            {
                ChangedEntries = ChangedEntries.ToArray(),
                RemovedEntries = DeletedEntries.ToArray()
            };

            ChangedEntries.Clear();
            DeletedEntries.Clear();

            return returned;
        }
    }
}
