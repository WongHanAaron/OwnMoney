using LiteDB;
using OwnMoney.Shared.Models.Monetary;
using OwnMoney.Shared.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwnMoney.Shared.Components.MoneyRepositories
{
    public static class MoneyQueryableExtensions
    {
        public static IQueryable<Entry> Filter(this IQueryable<Entry> entries, GetEntriesQuery filter)
        {
            return entries.Where(e => MatchesQuery(filter, e));
        }

        public static bool MatchesQuery(this GetEntriesQuery filter, Entry entry) 
        {
            return filter.Start.HasValue && entry.DateTime > filter.Start.Value &&
                   filter.End.HasValue && entry.DateTime < filter.End.Value;
        }
    }
}
