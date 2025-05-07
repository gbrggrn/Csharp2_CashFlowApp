using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    public class AccountManager
    {
        internal ObservableCollection<Account> Accounts { get; }

        public AccountManager()
        {
            Accounts = new ObservableCollection<Account>();
        }

        public void AddAccount(string accountName)
        {
            var newAccount = new Account { Name = accountName };
            Accounts.Add(newAccount);
            Console.WriteLine($"Account added {accountName}");
        }

        internal void RemoveAccount(int index)
        {
            Accounts.RemoveAt(index);
        }

        internal void EditAccountName(int index, string newAccountName)
        {
            Accounts[index].Name = newAccountName;
        }
    }
}
