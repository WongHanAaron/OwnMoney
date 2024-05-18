using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Requests
{
    public class GetEntriesQuery
    {
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }
    }
}
