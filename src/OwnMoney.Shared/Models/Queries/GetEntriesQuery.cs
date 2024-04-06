using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Queries
{
    public class GetEntriesQuery
    {
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}
