using OwnMoney.Shared.Models.Monetary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OwnMoney.Ui.Views.Transactions
{
    public class TransactionsViewModel
    {
        protected readonly IFilePicker _filePicker;

        public TransactionsViewModel(IFilePicker filePicker)
        {
            _filePicker = filePicker;
        }

        public ObservableCollection<Transaction> Transactions { get; set; } = new ObservableCollection<Transaction>();

    }
}
