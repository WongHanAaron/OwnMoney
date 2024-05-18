using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using OwnMoney.Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OwnMoney.Shared.Components.MoneyRepositories
{
    ///<summary> An interface to interact with the monetary repository </summary>
    public interface IMoneyRepository
    {
        ///<summary> Retrieve the entries from this repository </summary>
        Task<IEnumerable<Entry>> GetEntries(GetEntriesQuery query);

        ///<summary> Perform an upsert of the following entries </summary>
        Task UpsertEntries(IEnumerable<Entry> entries);

        ///<summary> Delete the following entries </summary>
        Task DeleteEntries(IEnumerable<long> entries);

        ///<summary> Subscribe to repository changes </summary>
        Task<SubscriptionResponse> SubscribeToChanges(SubscriptionRequest request);

        ///<summary> Unsubscribe from changes </summary>
        void UnsubscribeFromChanges(Guid requestId);

    }
}
