using Csharp2_CashFlowApp.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Csharp2_CashFlowApp.Model;
using Csharp2_CashFlowApp.Helpers;
using System.ComponentModel;
using System.DirectoryServices;

namespace Csharp2_CashFlowApp
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public ObservableCollection<Transaction> SearchableTransactions { get; }
        public ObservableCollection<Transaction> SearchResults { get; }

        public SearchWindow(ObservableCollection<Transaction> searchableTransactionsIn)
        {
            InitializeComponent();
            DataContext = this;
            SearchableTransactions = new ObservableCollection<Transaction>();
            SearchResults = new ObservableCollection<Transaction>();
            LoadComboBoxes();
            LoadSearchableTransactions(searchableTransactionsIn);
        }

        private void LoadSearchableTransactions(ObservableCollection<Transaction> searchableTransactions)
        {
            foreach (Transaction transaction in searchableTransactions)
            {
                SearchableTransactions.Add(transaction);
                SearchResults.Add(transaction);
            }
        }

        private void LoadComboBoxes()
        {
            monthComboBox.ItemsSource = Enum.GetValues(typeof(Enums.Months));
            categoryNameComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryName));
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Do you want to return to main window?", "Return?"))
            {
                this.Close();
            }
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBox.Clear();
            descriptionRadioBtn.IsChecked = false;
            categoryNameComboBox.SelectedIndex = -1;
            dateRadioBtn.IsChecked = false;
            expensesCheckBox.IsChecked = false;
            revenuesCheckBox.IsChecked = false;
            monthComboBox.SelectedIndex = 13;

            SearchResults.Clear();

            foreach (Transaction transaction in SearchableTransactions)
            {
                SearchResults.Add(transaction);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTxtBox.Text;

            SearchResults.Clear();

            foreach (Transaction transaction in SearchableTransactions)
            {
                bool match = true;

                if (descriptionRadioBtn.IsChecked == true || dateRadioBtn.IsChecked == true)
                {
                    if (descriptionRadioBtn.IsChecked == true && !transaction.Description.Contains(searchTerm, StringComparison.Ordinal))
                    {
                        match = false;
                    }
                    if (dateRadioBtn.IsChecked == true && !transaction.Date.ToShortDateString().Equals(searchTerm))
                    {
                        match = false;
                    }
                }

                if (categoryNameComboBox.SelectedIndex != -1)
                {
                    if (!transaction.Category!.CategoryName.Equals((Enums.CategoryName)categoryNameComboBox.SelectedItem))
                    {
                        match = false;
                    }
                }

                if (revenuesCheckBox.IsChecked == true || expensesCheckBox.IsChecked == true)
                {
                    if (revenuesCheckBox.IsChecked == true && transaction.Category!.CategoryType != Enums.CategoryType.Revenue)
                    {
                        match = false;
                    }
                    if (expensesCheckBox.IsChecked == true && transaction.Category!.CategoryType != Enums.CategoryType.Expense)
                    {
                        match = false;
                    }
                }

                if (monthComboBox.SelectedIndex > 0)
                {
                    if (transaction.Date.Month != monthComboBox.SelectedIndex)
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    SearchResults.Add(transaction);
                }
            }
        }
    }
}
