using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Requests
{
    public class GetEntriesQuery
    {
        public DateTimeOffset? Start { get; set; }
        public DateTimeOffset? End { get; set; }

        public override bool Equals(object obj)
        {
            return obj is GetEntriesQuery query &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(Start, query.Start) &&
                   EqualityComparer<DateTimeOffset?>.Default.Equals(End, query.End);
        }

        public override int GetHashCode()
        {
            int hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            return hashCode;
        }
    }
}
