using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    /// <summary>
    /// Responsible for generating reports.
    /// </summary>
    internal class ReportGenerator
    {
        private AccountManager accountManager;
        private int accountIndex;

        /// <summary>
        /// Constructur initializes.
        /// </summary>
        /// <param name="accountManagerIn">The main accountManager</param>
        /// <param name="accountIndexIn">Index of the account to be reported on</param>
        public ReportGenerator(AccountManager accountManagerIn, int accountIndexIn)
        {
            accountManager = accountManagerIn;
            accountIndex = accountIndexIn;
        }

        /// <summary>
        /// Generates the data for a full year report.
        /// </summary>
        /// <returns>The data as a dictionary</returns>
        internal Dictionary<DateTime, (double Revenue, double Expense, double CashFlow)> GenerateFullYearData()
        {
            //Copy transactions list for the account
            List<Transaction> transactions = accountManager.Accounts[accountIndex].TransactionManager.TransactionEntries.ToList();

            //Declare new dictionary
            Dictionary<DateTime, (double Revenue, double Expense, double CashFlow)> monthlySummary = [];

            //Iterate over transactions
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

        /// <summary>
        /// Generates data for a monthly report.
        /// </summary>
        /// <returns>The data as a dictionary</returns>
        internal List<MonthReport> GenerateMonthlyData()
        {
            //Retrieve transactions for the account
            List<Transaction> transactions = accountManager.Accounts[accountIndex].TransactionManager.TransactionEntries.ToList();
            
            //Declare new dictionary
            Dictionary<DateTime, (Dictionary<string, double> Revenues, Dictionary<string, double> Expenses, double NetCashFlow)> sortingStructure = [];

            //Iterate over transactions
            foreach (Transaction transaction in transactions)
            {
                var key = new DateTime(transaction.Date.Year, transaction.Date.Month, 1);

                //If transactions contains any transaction from the first day of a month
                if (!sortingStructure.ContainsKey(key))
                {
                    //Add new empty element
                    sortingStructure[key] = (new Dictionary<string, double>(), new Dictionary<string, double>(), 0.0);
                }

                //Store element for reference
                var current = sortingStructure[key];

                //Check categorytype
                if (transaction.Category?.CategoryType == Enums.CategoryType.Revenue)
                {
                    //Add categoryName to dictionary or fallback value "Unknown"
                    var category = transaction.Category?.CategoryName.ToString() ?? Enums.CategoryName.Unknown.ToString();
                    //Check if dictionary contains the categoryName already
                    if (current.Revenues.ContainsKey(category))
                    {
                        //If categoryName exists: add amount
                        current.Revenues[category] += transaction.Amount;
                    }
                    else
                    {
                        //Else create new category
                        current.Revenues[category] = transaction.Amount;
                    }
                }
                //If not revenue - is expense
                else
                {
                    var category = transaction.Category?.CategoryName.ToString() ?? Enums.CategoryName.Unknown.ToString();
                    if (current.Expenses.ContainsKey(category))
                    {
                        current.Expenses[category] += transaction.Amount;
                    }
                    else
                    {
                        current.Expenses[category] = transaction.Amount;
                    }
                }

                current.NetCashFlow += transaction.Amount;
                sortingStructure[key] = current;
            }

            return GenerateFormattedReport(sortingStructure);
        }

        /// <summary>
        /// Generates a formatted report from GenerateMonthlyData.
        /// </summary>
        /// <param name="sortedStructure">The sorted report data</param>
        /// <returns></returns>
        private List<MonthReport> GenerateFormattedReport(Dictionary<DateTime, (Dictionary<string, double> Revenues, Dictionary<string, double> Expenses, double NetCashFlow)> sortedStructure)
        {
            List<MonthReport> monthReports = [];

            foreach (var monthData in sortedStructure)
            {
                var key = monthData.Key;
                var value = monthData.Value;

                //Sort revenues/expenses by descending order, take the top 3 and save the respective keys (categoryName)
                var top3Revenues = value.Revenues.OrderByDescending(e => e.Value).Take(3).Select(e => $"{e.Key}").ToList();
                var top3Expenses = value.Expenses.OrderByDescending(e => e.Value).Take(3).Select(e => $"{e.Key}").ToList();

                double cashFlow = value.Revenues.Values.Sum() + value.Expenses.Values.Sum();

                var report = new MonthReport
                {
                    Month = key.ToString("MMMM yyyy"),
                    Top3Revenues = top3Revenues,
                    Top3Expenses = top3Expenses,
                    NetCashFlow = cashFlow
                };

                monthReports.Add(report);
            }

            return monthReports;
        }
    }
}
