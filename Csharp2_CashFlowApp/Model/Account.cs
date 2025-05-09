using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Control;

namespace Csharp2_CashFlowApp.Model
{
    /// <summary>
    /// Defines an Account.
    /// </summary>
    [Serializable]
    public class Account : INotifyPropertyChanged
    {
        //Variables
        public string name = string.Empty;
        private double balance;

        //Events
        public event PropertyChangedEventHandler? PropertyChanged;

        //Properties
        public TransactionManager TransactionManager { get; set; }
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public double Balance
        {
            get => balance;
            set
            {
                if (balance != value)
                {
                    balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        /// <summary>
        /// Constructor initializes a new transactionManager for this account.
        /// Subscribes to UpdateBalance upon changes of the transactionentries within its transactionManager.
        /// </summary>
        public Account()
        {
            TransactionManager = new();
            //Observe changes to transaction collection, upon change:
            //Pass sender/args to eventhandler and call UpdateBalance()
            TransactionManager.TransactionEntries.CollectionChanged += (s, e) => UpdateBalance();
        }

        /// <summary>
        /// Updates the balance of this account.
        /// </summary>
        private void UpdateBalance()
        {
            double totalRevenue = 0.0;
            double totalExpenses = 0.0;

            foreach (Transaction transaction in TransactionManager.TransactionEntries)
            {
                if (transaction.Category?.CategoryType == Enums.CategoryType.Revenue)
                {
                    totalRevenue += transaction.Amount;
                }
                else
                {
                    totalExpenses += transaction.Amount;
                }
            }

            Balance = totalRevenue - totalExpenses;
        }

        /// <summary>
        /// Constructor apparently does not run when deserializing json so
        /// this method rewires the collectionchanged event.
        /// </summary>
        public void RewireEvents()
        {
            TransactionManager.TransactionEntries.CollectionChanged += (s, e) => UpdateBalance();
        }

        /// <summary>
        /// Event helper.
        /// </summary>
        /// <param name="propertyName">Name of the property that triggered the event</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
