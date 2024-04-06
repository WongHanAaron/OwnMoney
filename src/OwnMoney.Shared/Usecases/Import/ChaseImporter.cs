using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace OwnMoney.Shared.Usecases.Import
{
    ///<summary> Responsible for importing an exported account details from  </summary>
    public interface IChaseImporter
    {

        void Import(DataTable table, string accountId);
    }

    public class ChaseImporter
    {

    }
}
