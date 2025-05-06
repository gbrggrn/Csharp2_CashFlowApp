using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Model
{
    internal class TransactionDTO
    {
        DateTime DateTimeTransfer {  get; set; }
        double AmountTransfer { get; set; }
        TransactionEntry.Category? CategoryTransfer {  get; set; }
        string Description { get; set; } = string.Empty;
    }
}
