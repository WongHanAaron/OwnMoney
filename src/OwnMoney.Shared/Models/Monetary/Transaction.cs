using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Monetary
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SourceId { get; set; }
        public int SourceObjectId { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public float Amount { get; set; }
    }
}
