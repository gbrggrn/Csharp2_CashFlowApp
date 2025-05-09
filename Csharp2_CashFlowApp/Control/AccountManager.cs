using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    [Serializable]
    /// <summary>
    /// Responsible for managing the Accounts-collection.
    /// </summary>
    public class AccountManager
    {
        //Properties
        public ObservableCollection<Account> Accounts { get; set; }

        /// <summary>
        /// Constructor initializes the collection.
        /// </summary>
        public AccountManager()
        {
            Accounts = new ObservableCollection<Account>();
        }

        /// <summary>
        /// Adds an account to the Accounts-collection.
        /// </summary>
        /// <param name="accountName">Name of the account to be added</param>
        public void AddAccount(string accountName)
        {
            var newAccount = new Account { Name = accountName };
            Accounts.Add(newAccount);
        }

        /// <summary>
        /// Removes an account from the Account-collection
        /// </summary>
        /// <param name="index">Index of the account to be removed</param>
        internal void RemoveAccount(int index)
        {
            Accounts.RemoveAt(index);
        }

        /// <summary>
        /// Edits the name of an account from the Account-Collection.
        /// </summary>
        /// <param name="index">Index of the account to be edited</param>
        /// <param name="newAccountName">New name of the account</param>
        internal void EditAccountName(int index, string newAccountName)
        {
            Accounts[index].Name = newAccountName;
        }
    }
}
