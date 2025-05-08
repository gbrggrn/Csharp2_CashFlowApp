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
        private AccountManager accountManager;
        private ReportGenerator reportGenerator;
        private int accountIndex;

        public ReportWindow(AccountManager accountManagerIn, int accountIndexIn)
        {
            InitializeComponent();
            accountManager = accountManagerIn;
            accountIndex = accountIndexIn;
            reportGenerator = new(accountManager, accountIndex);
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Return to main window?", "Return?"))
            {
                this.Close();
            }
        }

        private void FullYearBtn_Click(object sender, RoutedEventArgs e)
        {
            var fullYearReportData = reportGenerator.GenerateFullYearReport();

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

        private void DetailMonthBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
