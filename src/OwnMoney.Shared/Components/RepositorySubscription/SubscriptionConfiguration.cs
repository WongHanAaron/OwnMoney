using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Components.RepositorySubscription
{
    public class SubscriptionConfiguration
    {
        ///<summary> Flag for if the subscription should be enabled </summary>
        public bool EnableUpdate { get; set; } = false;

        ///<summary> How long a subscriber can subscribe for </summary>
        public int SubscriptionLifeTimeSeconds { get; set; }

        ///<summary> The frequency to check for updating the subscibers in milliseconds </summary>
        public int UpdateCheckPeriodMs { get; set; }
    }
}
