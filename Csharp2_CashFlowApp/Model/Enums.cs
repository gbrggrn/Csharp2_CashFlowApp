using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    public class Enums
    {
        public enum CategoryType
        {
            Expense,
            Revenue
        }

        public enum CategoryName
        {
            Unknown,
            Food,
            Transport,
            Restaurant,
            Beer,
            Bribe,
            Clothes,
            Car,
            Gift
        }

        public enum SortBy
        {
            Month,
            CategoryName,
            CategoryType,
            Amount
        }

        public enum Months
        {
            All,
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December,
        }
    }
}
