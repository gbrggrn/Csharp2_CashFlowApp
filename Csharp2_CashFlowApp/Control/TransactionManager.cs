using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csharp2_CashFlowApp.Model;

namespace Csharp2_CashFlowApp.Control
{
    public class TransactionManager
    {
        //Transaction-list
        public ObservableCollection<Transaction> transactionEntries;

        public TransactionManager()
        {
            transactionEntries = new ObservableCollection<Transaction>();
        }
        
        internal void AddTransaction(TransactionDTO transactionDTO)
        {
            Category newCategory = new()
            {
                Name = transactionDTO.CategoryName,
                CategoryType = transactionDTO.CategoryType,
            };

            Transaction newTransaction = new()
            {
                Date = transactionDTO.DateTimeTransfer,
                Amount = transactionDTO.AmountTransfer,
                Category = newCategory,
                Description = transactionDTO.Description
            };

            transactionEntries.Add(newTransaction);
        }
    }
}
