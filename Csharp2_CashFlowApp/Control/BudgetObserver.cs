using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Helpers;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    class BudgetObserver
    {
        public ObservableCollection<Account> Accounts { get; set; }
        private ObservableCollection<Budget> BudgetTracker {  get; set; }
        private readonly AccountManager accountManager;
        private readonly BudgetManager budgetManager;
        private readonly MainWindow mainWindow;

        public BudgetObserver(MainWindow mainWindowIn, AccountManager accountManagerIn, BudgetManager budgetManagerIn)
        {
            this.budgetManager = budgetManagerIn;
            this.accountManager = accountManagerIn;
            this.mainWindow = mainWindowIn;
            Accounts = accountManager!.Accounts;
            accountManager.Accounts.CollectionChanged += SubToNewAccount;
        }

        private void SubToNewAccount(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (accountManager.Accounts.Any())
            {
                var lastAccount = accountManager.Accounts.Last();
                lastAccount.TransactionManager.TransactionEntries.CollectionChanged += ObserveBudget;
            }
        }

        public void RewireSubsUponLoad()
        {
            foreach (var account in Accounts)
            {
                account.TransactionManager.TransactionEntries.CollectionChanged += ObserveBudget;
            }
        }

        public void EvaluateAllTransactions()
        {
            foreach (var account in accountManager.Accounts)
            {
                foreach (var transaction in account.TransactionManager.TransactionEntries)
                {
                    var categoryName = transaction.Category.CategoryName;
                    int month = transaction.Date.Month;

                    double aggregatedExpenses = CalculateExpenses(month, categoryName);

                    foreach (var budget in budgetManager.Budgets)
                    {
                        if (budget.Month.Equals(month) && budget.CategoryName.Equals(categoryName))
                        {
                            if (aggregatedExpenses > budget.CategoryBudget && budget.CategoryBudget != 0)
                            {
                                mainWindow.BudgetExceeded($"Budget for {budget.CategoryName} is currently exceeded!", "WARNING!");
                            }
                        }
                    }
                }
            }
        }

        private void ObserveBudget(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (accountManager.Accounts.Any())
            {
                var lastAccount = accountManager.Accounts.Last();
                if (lastAccount.TransactionManager != null && 
                    lastAccount.TransactionManager.TransactionEntries.Any())
                {
                    var lastEntry = lastAccount.TransactionManager.TransactionEntries.Last();
                    var categoryName = lastEntry.Category.CategoryName;
                    var categoryValue = lastEntry.Amount;
                    var categoryType = lastEntry.Category.CategoryType;
                    int month = lastEntry.Date.Month;

                    double aggregatedExpenses = CalculateExpenses(month, categoryName);

                    foreach (var budget in budgetManager.Budgets)
                    {
                        if (budget.Month.Equals(month) && budget.CategoryName.Equals(categoryName))
                        {
                            if (aggregatedExpenses > budget.CategoryBudget && budget.CategoryBudget != 0)
                            {
                                mainWindow.BudgetExceeded($"Budget for {budget.CategoryName} is currently exceeded!", "WARNING!");
                            }
                        }
                    }
                }
            }
        }

        private double CalculateExpenses(int month, Enums.CategoryName categoryName)
        {
            double total = 0;

            foreach (var account in accountManager.Accounts)
            {
                foreach (var transaction in account.TransactionManager.TransactionEntries)
                {
                    if (transaction.Date.Month.Equals(month) && transaction.Category.CategoryName.Equals(categoryName))
                    {
                        total += transaction.Amount;
                    }
                }
            }

            return total;
        }
    }
}
