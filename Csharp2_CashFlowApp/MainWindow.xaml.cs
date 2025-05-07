using Csharp2_CashFlowApp.Control;
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

        public MainWindow()
        {
            accountManager = new();
            dataContextSwitchYard = new(accountManager);
            DataContext = dataContextSwitchYard;
            InitializeComponent();
            accsListView.SelectionChanged += ToggleTransactionInputControls;
        }

        private void AddAccBtn_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"Add account button clicked {accNameTxtBox.Text}");
            accountManager.AddAccount(accNameTxtBox.Text);
            accNameTxtBox.Clear();
        }

        private void DeleteAccBtn_Click(object sender, RoutedEventArgs e)
        {
            if (accsListView.SelectedIndex != -1)
            {
                int index = accsListView.SelectedIndex;
                //ADD OPTION YES/NO
                accountManager.RemoveAccount(index);
            }
        }

        private void EditAccBtn_Click(object sender, RoutedEventArgs e)
        {
            //Add change
        }

        private void AddTransactionBtn_Click(object sender, RoutedEventArgs e)
        {
            
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
    }
}