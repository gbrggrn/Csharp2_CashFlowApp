using Csharp2_CashFlowApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Csharp2_CashFlowApp.Control;

namespace Csharp2_CashFlowApp
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        //Variables
        private AccountManager accountManager;
        private ReportGenerator reportGenerator;
        private int accountIndex;

        /// <summary>
        /// Constructor initializes instance variables.
        /// </summary>
        /// <param name="accountManagerIn">The main accountManager</param>
        /// <param name="accountIndexIn">Index of the account to generate reports from</param>
        public ReportWindow(AccountManager accountManagerIn, int accountIndexIn)
        {
            InitializeComponent();
            accountManager = accountManagerIn;
            accountIndex = accountIndexIn;
            reportGenerator = new(accountManager, accountIndex);
        }

        /// <summary>
        /// Reacts to exit button click. Closes window depending on choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Return to main window?", "Return?"))
            {
                this.Close();
            }
        }

        /// <summary>
        /// Retrieves data for the full year report.
        /// Formats the report view to accept the data.
        /// Formats the rows to be viewed.
        /// Sets itemssource of the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullYearBtn_Click(object sender, RoutedEventArgs e)
        {
            var fullYearReportData = reportGenerator.GenerateFullYearData();

            reportGridView.Columns.Clear();

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Month",
                Width = 100,
                DisplayMemberBinding = new Binding("Month")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Revenue",
                Width = 100,
                DisplayMemberBinding = new Binding("Revenue")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Expense",
                Width = 100,
                DisplayMemberBinding = new Binding("Expense")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Cash flow",
                Width = 100,
                DisplayMemberBinding = new Binding("CashFlow")
            });

            var reportRows = fullYearReportData.Select(kvp => new
            {
                Month = kvp.Key.ToString("MMMM yyyy"),
                Revenue = kvp.Value.Revenue.ToString(),
                Expense = kvp.Value.Expense.ToString(),
                CashFlow = kvp.Value.CashFlow.ToString()
            }).ToList();

            reportListView.ItemsSource = reportRows;
        }

        /// <summary>
        /// Retrieves the monthly summary data.
        /// Formats the report view to accept the data.
        /// Sets the itemssource for the listview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailMonthBtn_Click(object sender, RoutedEventArgs e)
        {
            var monthlySummary = reportGenerator.GenerateMonthlyData();

            reportGridView.Columns.Clear();

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Month",
                Width = 100,
                DisplayMemberBinding = new Binding("Month")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Top 3 expenses",
                Width = 100,
                DisplayMemberBinding = new Binding("Top3ExpensesDisplay")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Top 3 revenues",
                Width = 100,
                DisplayMemberBinding = new Binding("Top3RevenuesDisplay")
            });

            reportGridView.Columns.Add(new GridViewColumn
            {
                Header = "Cash flow",
                Width = 100,
                DisplayMemberBinding = new Binding("NetCashFlow")
            });

            reportListView.ItemsSource = monthlySummary;
        }
    }
}
