using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    [Serializable]
    public class TransactionManager
    {
        //Transaction-collection
        public ObservableCollection<Transaction> TransactionEntries { get; set; }

        public TransactionManager()
        {
            TransactionEntries = [];
        }

        internal void AddTransaction(TransactionDTO transactionDTO)
        {
            Category newCategory = new()
            {
                CategoryName = transactionDTO.CategoryNameTransfer,
                CategoryType = transactionDTO.CategoryTypeTransfer
            };

            Transaction newTransaction = new()
            {
                Date = transactionDTO.DateTimeTransfer,
                Amount = transactionDTO.AmountTransfer,
                Category = newCategory,
                Description = transactionDTO.DescriptionTransfer
            };

            TransactionEntries.Add(newTransaction);
        }
    }
}
