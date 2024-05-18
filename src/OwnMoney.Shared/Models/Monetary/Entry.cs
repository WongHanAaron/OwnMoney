using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Monetary
{
    ///<summary> Represents a base element of an entry in a ledger </summary>
    public class Entry
    {
        [BsonId]
        public long Id { get; set; }
        public string SourceId { get; set; }
        public string SourceObjectId { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
    }
}
