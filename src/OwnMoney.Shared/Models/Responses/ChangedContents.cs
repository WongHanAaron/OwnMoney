using System;
using System.Collections.Generic;
using System.Text;

namespace OwnMoney.Shared.Models.Responses
{
    ///<summary> Contains the entries that have changed </summary>
    public class ChangedContents
    {
        ///<summary> The entries that have been changed </summary>
        public long[] ChangedEntries { get; set; }

        ///<summary> The entries that have been removed </summary>
        public long[] RemovedEntries { get; set; }
    }
}
