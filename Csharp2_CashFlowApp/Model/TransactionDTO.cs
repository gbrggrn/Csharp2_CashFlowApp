using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    /// <summary>
    /// Defines a DTO of a transaction.
    /// </summary>
    public class TransactionDTO
    {
        public Enums.CategoryName CategoryNameTransfer { get; set; }
        public Enums.CategoryType CategoryTypeTransfer { get; set; }
        public DateTime DateTimeTransfer {  get; set; }
        public double AmountTransfer { get; set; }
        public string DescriptionTransfer { get; set; } = string.Empty;
    }
}
