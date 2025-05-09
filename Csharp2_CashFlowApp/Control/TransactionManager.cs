using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        internal void SortTransactions(Enums.SortBy sortByIn)
        {
            //Instantiate TransactionSorter with sorting instruction
            TransactionSorter transactionSorter = new(sortByIn);

            //Clear transactionEntries (that is observed by the UI)
            TransactionEntries.Clear();

            //Sort the transactions to a list
            List<Transaction> sortedTransactions = TransactionEntries.OrderBy(transaction => transaction, transactionSorter).ToList();

            //Add the transactions to transactionEntries-collection
            foreach (Transaction transaction in sortedTransactions)
            {
                TransactionEntries.Add(transaction);
            }
        }
    }
}
