using Csharp2_CashFlowApp.Control;
using Csharp2_CashFlowApp.Model;
using Csharp2_CashFlowApp.Helpers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Reflection.Metadata;
using System.Collections.ObjectModel;

namespace Csharp2_CashFlowApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Variables
        private readonly AccountManager accountManager;
        private readonly AccountTransactionObserver accountTransactionObserver;
        private readonly FileManager fileManager;
        private readonly BudgetManager budgetManager;
        private readonly BudgetObserver budgetObserver;

        //Constants
        private const int maxCharLength = 20;

        /// <summary>
        /// Constructor initializes instance variables, populates UI elements and sets subscription.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            fileManager = new();
            accountManager = new();
            budgetManager = new();
            budgetObserver = new(this, accountManager, budgetManager);
            accountTransactionObserver = new(accountManager);
            DataContext = accountTransactionObserver;
            accsListView.SelectionChanged += ToggleTransactionInputControls;
            LoadCategories();
        }

        /// <summary>
        /// Displays errormessage upon exceeding of budget.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="title">Title of the messagebox</param>
        public void BudgetExceeded(string message, string title)
        {
            MessageBoxes.DisplayInfoBox(message, title);
        }
        
        /// <summary>
        /// Loads category types and names into respective comboboxes
        /// </summary>
        private void LoadCategories()
        {
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryType));
            categoryComboBox.SelectedIndex = 0;

            categoryNameComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryName));
            categoryNameComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Reacts to add button click. Adds an account to the account collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAccBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(accNameTxtBox.Text))
            {
                MessageBoxes.DisplayErrorBox("Account name can not be empty!");
            }
            else if (accNameTxtBox.Text.Length > maxCharLength)
            {
                MessageBoxes.DisplayErrorBox($"Account name can not be longer than {maxCharLength}");
            }
            else
            {
                accountManager.AddAccount(accNameTxtBox.Text);
                accNameTxtBox.Clear();
            }
        }

        /// <summary>
        /// Reacts to delete button click. Deletes an account from the account collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAccBtn_Click(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                if (MessageBoxes.DisplayQuestion("Delete this account and all transactions?",
                    "Delete: are you sure?"))
                {
                    accountManager.RemoveAccount(index);
                }
            }
        }

        private void EditAccBtn_Click(object sender, RoutedEventArgs e)
        {
            //Not implemented
        }

        /// <summary>
        /// Adds a transaction to the transaction collection in the transactionmanager of an account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTransactionBtn_Click(object sender, RoutedEventArgs e)
        {
            List<string> errorMessages = ValidateTransactionInput();

            if (errorMessages.Count > 0)
            {
                MessageBoxes.DisplayErrorBox($"Input errors:\n{errorMessages}");
            }
            else
            {
                int index = accsListView.SelectedIndex;

                TransactionDTO transactionDTO = new()
                {
                    CategoryNameTransfer = (Enums.CategoryName)categoryNameComboBox.SelectedItem,
                    CategoryTypeTransfer = (Enums.CategoryType)categoryComboBox.SelectedItem,
                    DateTimeTransfer = (DateTime)datePicker.SelectedDate!,
                    AmountTransfer = double.Parse(amountTxtBox.Text),
                    DescriptionTransfer = descriptionTxtBox.Text
                };

                accountManager.Accounts[index].TransactionManager.AddTransaction(transactionDTO);
                ClearTransactionInputControls();
            }
        }

        /// <summary>
        /// Validates transaction input.
        /// </summary>
        /// <returns>A list of errormessages</returns>
        private List<string> ValidateTransactionInput()
        {
            List<string> errorMessages = [];

            if (!datePicker.SelectedDate.HasValue)
            {
                errorMessages.Add("You need to pick a date for the transaction");
            }

            if (!string.IsNullOrWhiteSpace(amountTxtBox.Text))
            {
                if (!double.TryParse(amountTxtBox.Text, out double result))
                {
                    _ = result;
                    errorMessages.Add("Amount needs to be entered as a double");
                }
            }
            else
            {
                errorMessages.Add("Amount can not be empty!");
            }

            if (!string.IsNullOrWhiteSpace(descriptionTxtBox.Text))
            {
                if (descriptionTxtBox.Text.Length > maxCharLength)
                {
                    errorMessages.Add($"Description can not be longer than {maxCharLength}!");
                }
            }
            else
            {
                errorMessages.Add("Description can not be emtpy");
            }

            return errorMessages;
        }

        /// <summary>
        /// Toggles input controls upon account selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleTransactionInputControls(object sender, RoutedEventArgs e)
        {
            datePicker.IsEnabled = true;
            categoryComboBox.IsEnabled = true;
            categoryNameComboBox.IsEnabled = true;
            amountTxtBox.IsEnabled = true;
            descriptionTxtBox.IsEnabled = true;
            addTransactionBtn.IsEnabled = true;
        }

        /// <summary>
        /// Opens the report-window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {
            if (transactionListView.Items.Count > 0)
            {
                int index = accsListView.SelectedIndex;
                ReportWindow reportWindow = new(accountManager, index);

                reportWindow.ShowDialog();
            }
            else
            {
                MessageBoxes.DisplayErrorBox("No transactions added to build reports from!");
            }
        }

        /// <summary>
        /// Clears transaction input controls.
        /// </summary>
        private void ClearTransactionInputControls()
        {
            categoryComboBox.SelectedIndex = 0;
            amountTxtBox.Clear();
            descriptionTxtBox.Clear();
        }

        /// <summary>
        /// Prompts the user for choice of closing the app.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Do you want to exit the application?\n" +
                "Unsaved changes will be lost", "Exit?"))
            {
                this.Close();
            }
        }

        /// <summary>
        /// Saves the currently added accounts + transactions in json format.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (save.ShowDialog() == true)
            {
                try
                {
                    fileManager.Serialize(save.FileName, accountManager);
                }
                catch (Exception ex)
                {
                    MessageBoxes.DisplayErrorBox(ex.Message);
                }

                MessageBoxes.DisplayInfoBox("Successful save!", "File saved");
            }
        }

        /// <summary>
        /// Loads content from a json file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new()
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"
            };

            if (open.ShowDialog() == true)
            {
                try
                {
                    //Deserialize
                    var newAccountManager = fileManager.Deserialize(open.FileName);

                    //Clear UI list
                    accountManager.Accounts.Clear();

                    //Add deserialized accounts to main accountManager
                    foreach (var account in newAccountManager.Accounts)
                    {
                        //Rewire events for each account
                        account.RewireEvents();
                        accountManager.Accounts.Add(account);
                    }

                    //Rewire budget observation
                    budgetObserver.RewireSubsUponLoad();
                    //Evaluate new transactions against budgets
                    budgetObserver.EvaluateAllTransactions();

                    //Clear main UI observed collection
                    accountTransactionObserver.ObservableAccounts.Clear();
                    //Re-add deserialized accounts
                    foreach (var account in newAccountManager.Accounts)
                    {
                        accountTransactionObserver.ObservableAccounts.Add(account);
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxes.DisplayErrorBox(ex.Message);
                }
            }
        }

        /// <summary>
        /// Opens the set limits window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetLimitsBtn_Click(object sender, RoutedEventArgs e)
        {
            SetLimitsWindow setLimitsWindow = new(budgetManager);

            setLimitsWindow.ShowDialog();

            //Evaluate existing transactions upon close
            budgetObserver.EvaluateAllTransactions();
        }

        /// <summary>
        /// Opens the search window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Transaction> allTransactions = [];

            //Copy all transactions
            foreach (Account account in accountManager.Accounts)
            {
                foreach (Transaction transaction in account.TransactionManager.TransactionEntries)
                {
                    allTransactions.Add(transaction);
                }
            }
            SearchWindow searchWindow = new(allTransactions);

            searchWindow.ShowDialog();
        }
    }
}