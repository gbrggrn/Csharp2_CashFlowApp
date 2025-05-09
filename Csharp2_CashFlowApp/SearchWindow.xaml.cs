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
        //Properties
        public ObservableCollection<Transaction> SearchableTransactions { get; }
        public ObservableCollection<Transaction> SearchResults { get; }

        /// <summary>
        /// Constructor initializes and loads UI elements.
        /// </summary>
        /// <param name="searchableTransactionsIn"></param>
        public SearchWindow(ObservableCollection<Transaction> searchableTransactionsIn)
        {
            InitializeComponent();
            DataContext = this;
            SearchableTransactions = [];
            SearchResults = [];
            LoadComboBoxes();
            LoadSearchableTransactions(searchableTransactionsIn);
        }

        /// <summary>
        /// Loads the searchable transactions.
        /// </summary>
        /// <param name="searchableTransactions">A collection of all transactions</param>
        private void LoadSearchableTransactions(ObservableCollection<Transaction> searchableTransactions)
        {
            foreach (Transaction transaction in searchableTransactions)
            {
                SearchableTransactions.Add(transaction);
                SearchResults.Add(transaction);
            }
        }

        /// <summary>
        /// Loads comboboxes values.
        /// </summary>
        private void LoadComboBoxes()
        {
            monthComboBox.ItemsSource = Enum.GetValues(typeof(Enums.Months));
            categoryNameComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryName));
        }

        /// <summary>
        /// Reacts to exit button click. Closes window depending on choice.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Do you want to return to main window?", "Return?"))
            {
                this.Close();
            }
        }

        /// <summary>
        /// Clears all search parameters.
        /// Repopulates the SearchResults collection so that all transactions are now visible again.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            searchTxtBox.Clear();
            descriptionRadioBtn.IsChecked = false;
            categoryNameComboBox.SelectedIndex = -1;
            datePicker.SelectedDate = DateTime.Now;
            expensesCheckBox.IsChecked = false;
            revenuesCheckBox.IsChecked = false;
            monthComboBox.SelectedIndex = 13;

            SearchResults.Clear();

            foreach (Transaction transaction in SearchableTransactions)
            {
                SearchResults.Add(transaction);
            }
        }

        /// <summary>
        /// Reacts to search button click.
        /// Checks what parameters are applied to the search and evaluates transactions on those.
        /// A boolean flag "match" is set to false if transaction does not match.
        /// If all statements evaluate true: adds the transaction to SearchResults for display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTxtBox.Text;

            SearchResults.Clear();

            foreach (Transaction transaction in SearchableTransactions)
            {
                bool match = true;

                if (descriptionRadioBtn.IsChecked == true)
                {
                    if (descriptionRadioBtn.IsChecked == true && !transaction.Description.Contains(searchTerm, StringComparison.Ordinal))
                    {
                        match = false;
                    }
                }

                if (datePicker.SelectedDate.HasValue)
                {
                    if (!transaction.Date.Equals(datePicker.SelectedDate))
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
