using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Monetary
{
    public class Transaction : Entry
    {
        ///<summary> The category assigned to this transaction </summary>
        public int CategoryId { get; set; }
        public TransactionType Type { get; set; }
    }
}
