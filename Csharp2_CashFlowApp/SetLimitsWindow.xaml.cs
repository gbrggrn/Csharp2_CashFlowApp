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
        //Variables
        private readonly BudgetManager budgetManager;

        /// <summary>
        /// Constructor initializes, sets subscription and calls initial budget update.
        /// </summary>
        /// <param name="budgetManagerIn">The main budgetManager</param>
        public SetLimitsWindow(BudgetManager budgetManagerIn)
        {
            InitializeComponent();
            budgetManager = budgetManagerIn;
            DataContext = budgetManager;
            budgetListView.SelectionChanged += DisplayBudgetOnSelection;
            LoadMonths();
            budgetManager.UpdateBudgets();
        }

        /// <summary>
        /// Populates the month combobox.
        /// </summary>
        private void LoadMonths()
        {
            monthComboBox.ItemsSource = Enum.GetValues(typeof(Enums.Months));
            monthComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Reacts to clear button click.
        /// Clears the selected budget.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            if (budgetListView.SelectedIndex != -1)
            {
                budgetManager.RemoveBudget(monthComboBox.SelectedIndex, 
                    budgetManager.CurrentBudgets[budgetListView.SelectedIndex].CategoryName);
            }
        }

        /// <summary>
        /// Reacts to save button click. Saves the entered budget to the selected category.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Displays the current budget in the input textbox upon selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Reacts to exit button click. Closes window depending on choice.
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
    }
}
