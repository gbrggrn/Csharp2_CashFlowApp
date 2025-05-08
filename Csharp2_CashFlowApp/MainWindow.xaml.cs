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

namespace Csharp2_CashFlowApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AccountManager accountManager;
        private readonly DataContextSwitchYard dataContextSwitchYard;

        private const int maxCharLength = 20;

        public MainWindow()
        {
            InitializeComponent();
            accountManager = new();
            dataContextSwitchYard = new(accountManager);
            DataContext = dataContextSwitchYard;
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

                accountManager.Accounts[index].transactionManager.AddTransaction(transactionDTO);
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
            categoryComboBox.IsEnabled = true;
            amountTxtBox.IsEnabled = true;
            descriptionTxtBox.IsEnabled = true;
            addTransactionBtn.IsEnabled = true;
        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearTransactionInputControls()
        {
            categoryComboBox.SelectedIndex = 0;
            amountTxtBox.Clear();
            descriptionTxtBox.Clear();
        }

        private void monthSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].transactionManager.SortTransactions(Enums.SortBy.Month);
            }
        }

        private void categoryTypeSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].transactionManager.SortTransactions(Enums.SortBy.CategoryType);
            }
        }

        private void categoryNameSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].transactionManager.SortTransactions(Enums.SortBy.CategoryName);
            }
        }

        private void amountSortRadioBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                accountManager.Accounts[index].transactionManager.SortTransactions(Enums.SortBy.Amount);
            }
        }
    }
}