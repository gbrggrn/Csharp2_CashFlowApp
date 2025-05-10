using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    /// <summary>
    /// Responsible for managing budgets.
    /// </summary>
    public class BudgetManager : INotifyPropertyChanged
    {
        public ObservableCollection<Budget> Budgets { get; }
        public ObservableCollection<Budget> currentBudgets;

        /// <summary>
        /// Handles changes to CurrentBudgets.
        /// CurrentBudgets is UI-observed by the listview in setLimitsWindow.
        /// </summary>
        public ObservableCollection<Budget> CurrentBudgets
        {
            get => currentBudgets;
            set
            {
                if (currentBudgets != value)
                {
                    currentBudgets = value;
                    OnPropertyChanged(nameof(CurrentBudgets));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int selectedMonth;

        /// <summary>
        /// Handles changes to selectedMonth.
        /// SelectedMonth is UI-bound to the monthComboBox of setLimitsWindow.
        /// </summary>
        public int SelectedMonth
        {
            get => selectedMonth;
            set
            {
                if (selectedMonth != value)
                {
                    selectedMonth = value;
                    OnPropertyChanged(nameof(SelectedMonth));
                    UpdateBudgets();
                }
            }
        }
        
        /// <summary>
        /// Constructor initializes.
        /// </summary>
        public BudgetManager()
        {
            Budgets = [];
            currentBudgets = [];
            PopulateBudgets();
            SelectedMonth = 0;
        }

        /// <summary>
        /// Updates the budgets of CurrentBudgets.
        /// </summary>
        public void UpdateBudgets()
        {
            CurrentBudgets.Clear();

            foreach (var budget in Budgets)
            {
                if (budget.Month == selectedMonth)
                {
                    CurrentBudgets.Add(budget);
                }
            }
        }

        /// <summary>
        /// Event helper.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed</param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Populates the Budgets collection with months, categoryNames and initial budget value of zero.
        /// </summary>
        private void PopulateBudgets()
        {
            foreach (var month in Enum.GetValues(typeof(Enums.Months))) 
            {
                int monthNumber = (int)month;
                Console.WriteLine(monthNumber);
                foreach (var categoryName in Enum.GetValues(typeof(Enums.CategoryName)).Cast<Enums.CategoryName>())
                {
                    Budgets.Add(new Budget { Month = monthNumber, CategoryName = categoryName, CategoryBudget = 0 });
                }
            }
        }

        /// <summary>
        /// Adds a new CategoryBudget to a Budget instance.
        /// </summary>
        /// <param name="month">The selected month</param>
        /// <param name="categoryName">The selected CategoryName</param>
        /// <param name="newBudget">The new budget value</param>
        public void AddBudget(int month, Enums.CategoryName categoryName, double newBudget)
        {
            foreach (var thisBudget in Budgets)
            {
                if (thisBudget.Month.Equals(month) && thisBudget.CategoryName.Equals(categoryName))
                {
                    thisBudget.CategoryBudget = newBudget;
                    break;
                }
            }

            //Call update to make UI react
            UpdateBudgets();
        }

        /// <summary>
        /// Removes, or rather resets a chosen CategoryBudget to zero.
        /// </summary>
        /// <param name="month">The selected month</param>
        /// <param name="categoryName">The selected categoryName</param>
        public void RemoveBudget(int month, Enums.CategoryName categoryName)
        {
            foreach (var thisBudget in Budgets)
            {
                if (thisBudget.Month.Equals(month) && thisBudget.CategoryName.Equals(categoryName))
                {
                    thisBudget.CategoryBudget = 0;
                    break;
                }
            }
        }
    }
}
