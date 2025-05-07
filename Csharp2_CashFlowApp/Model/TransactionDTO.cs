using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    public class TransactionDTO
    {
        public Enums.CategoryType CategoryType { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime DateTimeTransfer {  get; set; }
        public double AmountTransfer { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
