using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    internal class ReportGenerator
    {
        private AccountManager accountManager;
        private int accountIndex;

        public ReportGenerator(AccountManager accountManagerIn, int accountIndexIn)
        {
            accountManager = accountManagerIn;
        }

        internal Dictionary<DateTime, (double Revenue, double Expense, double CashFlow)> GenerateFullYearReport()
        {
            List<Transaction> transactions = accountManager.Accounts[accountIndex].transactionManager.transactionEntries.ToList();

            Dictionary<DateTime, (double Revenue, double Expense, double CashFlow)> monthlySummary = [];

            foreach (Transaction transaction in transactions)
            {
                var key = new DateTime(transaction.Date.Year, transaction.Date.Month, 1);

                //If transactions contains any transaction from the first day of a month
                if (!monthlySummary.ContainsKey(key))
                {
                    //Create new dictionary-element
                    monthlySummary[key] = (0, 0, 0);
                }

                //Store element for reference
                var current = monthlySummary[key];

                //Switch cases dependant on CategoryType (revenue or expense)
                switch (transaction.Category?.CategoryType)
                {
                    case Enums.CategoryType.Revenue:
                        current.Revenue += transaction.Amount;
                        current.CashFlow += transaction.Amount;
                        break;
                    case Enums.CategoryType.Expense:
                        current.Expense += transaction.Amount;
                        current.CashFlow -= transaction.Amount;
                        break;
                }

                //Save to dictionary-element
                monthlySummary[key] = current;
            }

            return monthlySummary;
        }
    }
}
