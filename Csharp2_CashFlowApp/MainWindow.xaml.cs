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
        private readonly AccountManager accountManager;
        private readonly AccountTransactionObserver accountTransactionObserver;
        private readonly FileManager fileManager;

        private const int maxCharLength = 20;

        public MainWindow()
        {
            InitializeComponent();
            fileManager = new();
            accountManager = new();
            accountTransactionObserver = new(accountManager);
            DataContext = accountTransactionObserver;
            accsListView.SelectionChanged += ToggleTransactionInputControls;
            LoadCategories();
        }
        
        private void LoadCategories()
        {
            categoryComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryType));
            categoryComboBox.SelectedIndex = 0;

            categoryNameComboBox.ItemsSource = Enum.GetValues(typeof(Enums.CategoryName));
            categoryNameComboBox.SelectedIndex = 0;
        }

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
            //Add change
        }

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
                Console.WriteLine("Transaction added");
            }
        }

        private List<string> ValidateTransactionInput()
        {
            List<string> errorMessages = [];

            if (datePicker.SelectedDate == null)
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

        private void ToggleTransactionInputControls(object sender, RoutedEventArgs e)
        {
            datePicker.IsEnabled = true;
            categoryComboBox.IsEnabled = true;
            categoryNameComboBox.IsEnabled = true;
            amountTxtBox.IsEnabled = true;
            descriptionTxtBox.IsEnabled = true;
            addTransactionBtn.IsEnabled = true;
        }

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

        private void ClearTransactionInputControls()
        {
            categoryComboBox.SelectedIndex = 0;
            amountTxtBox.Clear();
            descriptionTxtBox.Clear();
        }

        private void MonthSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].TransactionManager.SortTransactions(Enums.SortBy.Month);
            }
        }

        private void CategoryTypeSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].TransactionManager.SortTransactions(Enums.SortBy.CategoryType);
            }
        }

        private void CategoryNameSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].TransactionManager.SortTransactions(Enums.SortBy.CategoryName);
            }
        }

        private void AmountSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].TransactionManager.SortTransactions(Enums.SortBy.Amount);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Do you want to exit the application?\n" +
                "Unsaved changes will be lost", "Exit?"))
            {
                this.Close();
            }
        }

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
                    var newAccountManager = fileManager.Deserialize(open.FileName);

                    accountManager.Accounts.Clear();

                    foreach (var account in newAccountManager.Accounts)
                    {
                        accountManager.Accounts.Add(account);
                    }

                    accountTransactionObserver.ObservableAccounts.Clear();
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

        private void SetLimitsBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Transaction> allTransactions = [];

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