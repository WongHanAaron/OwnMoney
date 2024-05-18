using OwnMoney.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Components.RepositorySubscription
{
    public class SubscriberNotification
    {
        public Guid RequestId { get; set; }
        public ChangedContents Changes { get; set; }
    }
}
