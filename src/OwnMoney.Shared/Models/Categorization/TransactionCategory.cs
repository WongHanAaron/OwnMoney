using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Categorization
{
    ///<summary> Represents a category for a set of transactions </summary>
    public class TransactionCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
