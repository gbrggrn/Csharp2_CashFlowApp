using Csharp2_CashFlowApp.Control;
using Csharp2_CashFlowApp.Helpers;
using Csharp2_CashFlowApp.Model;
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

namespace Csharp2_CashFlowApp
{
    /// <summary>
    /// Interaction logic for SetLimitsWindow.xaml
    /// </summary>
    public partial class SetLimitsWindow : Window
    {
        private readonly BudgetManager budgetManager;

        public SetLimitsWindow(BudgetManager budgetManagerIn)
        {
            InitializeComponent();
            budgetManager = budgetManagerIn;
            DataContext = budgetManager;
            budgetListView.SelectionChanged += DisplayBudgetOnSelection;
            LoadMonths();
            budgetManager.UpdateBudgets();
        }

        private void LoadMonths()
        {
            monthComboBox.ItemsSource = Enum.GetValues(typeof(Enums.Months));
            monthComboBox.SelectedIndex = 0;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            if (budgetListView.SelectedIndex != -1)
            {
                budgetManager.RemoveBudget(monthComboBox.SelectedIndex, 
                    budgetManager.CurrentBudgets[budgetListView.SelectedIndex].CategoryName,
                    0);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (budgetListView.SelectedIndex != -1)
            {
                if (double.TryParse(budgetTxtBox.Text, out double result))
                {
                    budgetManager.AddBudget(monthComboBox.SelectedIndex,
                        budgetManager.CurrentBudgets[budgetListView.SelectedIndex].CategoryName,
                        result);
                }
                else
                {
                    MessageBoxes.DisplayErrorBox("New budget needs to be a number");
                }
            }
            else
            {
                MessageBoxes.DisplayErrorBox("You need to selecte a category first");
            }
        }

        private void DisplayBudgetOnSelection(object sender, SelectionChangedEventArgs e)
        {
            int index = budgetListView.SelectedIndex;

            if (index == -1 || index >= budgetManager.CurrentBudgets.Count)
            {
                return;
            }

            budgetTxtBox.Clear();
            budgetTxtBox.Text = budgetManager.CurrentBudgets[index].CategoryBudget.ToString();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxes.DisplayQuestion("Return to main window?", "Return?"))
            {
                this.Close();
            }
        }
    }
}
