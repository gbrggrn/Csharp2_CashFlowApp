using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    internal class TransactionEntry
    {
        public record Category
        {
            string Name { get; set; } = string.Empty;
            Enums.CategoryType CategoryType { get; set; }
        }

        public record Transaction
        {
            DateTime Date {  get; set; }
            double Amount { get; set; }
            Category? Category { get; set; }
            string Description { get; set; } = string.Empty;
        }
    }
}
