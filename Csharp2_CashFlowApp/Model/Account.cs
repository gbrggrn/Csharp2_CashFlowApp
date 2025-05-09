using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Control;

namespace Csharp2_CashFlowApp.Model
{
    public class Account : INotifyPropertyChanged
    {
        //Variables
        public string name;
        private double balance;
        public TransactionManager transactionManager;

        //Events
        public event PropertyChangedEventHandler? PropertyChanged;

        //Properties
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

        public Account()
        {
            transactionManager = new();
            //Observe changes to transaction collection, upon change:
            //Pass sender/args to eventhandler and call UpdateBalance()
            transactionManager.transactionEntries.CollectionChanged += (s, e) => UpdateBalance();
        }

        private void UpdateBalance()
        {
            double totalRevenue = 0.0;
            double totalExpenses = 0.0;

            foreach (Transaction transaction in transactionManager.transactionEntries)
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

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
