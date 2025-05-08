using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    public class DataContextSwitchYard : INotifyPropertyChanged
    {
        public ObservableCollection<Account> ObservableAccounts { get; }
        public ObservableCollection<Transaction> ObservableTransactions
        {
            get
            {
                if (SelectedAccount?.transactionManager.transactionEntries != null)
                {
                    Console.WriteLine("Transactions observed");
                    return SelectedAccount?.transactionManager.transactionEntries!;
                }
                else
                {
                    return new ObservableCollection<Transaction>();
                }
            }
        }

        public DataContextSwitchYard(AccountManager accountManagerIn)
        {
            Console.WriteLine("DataContextSwitchYard initialized");
            ObservableAccounts = accountManagerIn.Accounts;
        }

        private Account? selectedAccount;
        public Account? SelectedAccount
        {
            get => selectedAccount;
            set
            {
                if (selectedAccount != value)
                {
                    selectedAccount = value;
                    Console.WriteLine("SelectedAccountChanged");
                    //OnPropertyChanged(nameof(SelectedAccount));
                    OnPropertyChanged(nameof(ObservableTransactions));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
