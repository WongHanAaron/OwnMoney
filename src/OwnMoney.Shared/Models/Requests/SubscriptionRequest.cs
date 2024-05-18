using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Requests
{
    ///<summary> A request to subscribe to changes from the IMoneyRepository </summary>
    public class SubscriptionRequest
    {
        ///<summary> The unique ID for this request </summary>
        public Guid Id { get; protected set; } = Guid.NewGuid();

        ///<summary> The query performed and to subscribe to changes to </summary>
        public GetEntriesQuery Query { get; set; }

        ///<summary> The frequency in seconds on when to send updates if available </summary>
        public int FrequencySeconds { get; set; }
    }
}
