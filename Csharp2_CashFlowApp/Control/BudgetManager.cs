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
    public class BudgetManager : INotifyPropertyChanged
    {
        public ObservableCollection<Budget> Budgets { get; }
        public ObservableCollection<Budget> currentBudgets;
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

        public int SelectedMonth
        {
            get => selectedMonth;
            set
            {
                if (selectedMonth != value)
                {
                    selectedMonth = value;
                    Console.WriteLine(selectedMonth);
                    OnPropertyChanged(nameof(SelectedMonth));
                    UpdateBudgets();
                }
            }
        }
        
        public BudgetManager()
        {
            Budgets = [];
            currentBudgets = [];
            PopulateBudgets();
            SelectedMonth = 0;
        }

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

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

            UpdateBudgets();
        }

        public void RemoveBudget(int month, Enums.CategoryName categoryName, double newBudget)
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
