using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    public class Budget : INotifyPropertyChanged
    {
        public Enums.CategoryName CategoryName { get; set; }
        private int month;
        public int Month
        {
            get => month;
            set
            {
                if (month != value)
                {
                    month = value;
                    OnPropertyChanged(nameof(Month));
                }
            }
        }
        private double categoryBudget;
        public double CategoryBudget
        {
            get => categoryBudget;
            set
            {
                if (categoryBudget != value)
                {
                    categoryBudget = value;
                    OnPropertyChanged(nameof(CategoryBudget));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
