using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Repositories.MoneyRepositories
{
    ///<summary> An interface to interact with the monetary repository </summary>
    public interface IMoneyRepository
    {
        IEnumerable<Entry> GetEntries(GetEntriesQuery query);
        void InsertEntries(IEnumerable<Entry> entries);
    }
}
