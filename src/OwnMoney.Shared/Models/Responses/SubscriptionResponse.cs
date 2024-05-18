using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Responses
{
    public class SubscriptionResponse
    {
        ///<summary> The unique id for the request being made </summary>
        public Guid RequestId { get; set; }

        ///<summary> The lifetime of the subscription </summary>
        public int LifeTimeSeconds { get; set; }
    }
}
