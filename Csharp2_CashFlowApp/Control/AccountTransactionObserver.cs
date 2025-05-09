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
    /// <summary>
    /// Centre point for funneling updates to accounts and transactions to UI-layer.
    /// Basically a small-ish ViewModel.
    /// Responsible for switches between instances of TransactionManagers based on account-selection.
    /// </summary>
    public class AccountTransactionObserver : INotifyPropertyChanged
    {
        //Observes the Accounts collection in AccountManager
        public ObservableCollection<Account> ObservableAccounts { get; }
        //Observes the transaction entries collection in TransactionManager
        public ObservableCollection<Transaction> ObservableTransactions
        {
            get
            {
                //Get the transactionmanager of the selected account
                if (SelectedAccount?.transactionManager.transactionEntries != null)
                {
                    return SelectedAccount?.transactionManager.transactionEntries!;
                }
                else
                {
                    //If no transactionmanager found (unlikely) - return a new, empty instance
                    return new ObservableCollection<Transaction>();
                }
            }
        }
        
        /// <summary>
        /// Constructor initializes the observing collection observing accounts with
        /// the accounts-collection from AccountManager.
        /// </summary>
        /// <param name="accountManagerIn"></param>
        public AccountTransactionObserver(AccountManager accountManagerIn)
        {
            ObservableAccounts = accountManagerIn.Accounts;
        }

        //Nullable variable of type Account to hold a selected account
        private Account? selectedAccount;
        //Property that raises and event upon set if a different account is selected
        public Account? SelectedAccount
        {
            get => selectedAccount;
            set
            {
                if (selectedAccount != value)
                {
                    selectedAccount = value;
                    OnPropertyChanged(nameof(ObservableTransactions));
                }
            }
        }

        //Eventhandler
        public event PropertyChangedEventHandler? PropertyChanged;

        //Event helper, invokes the actual event
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
