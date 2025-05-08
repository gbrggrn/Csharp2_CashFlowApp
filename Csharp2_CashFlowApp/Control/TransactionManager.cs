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
        //Transaction-collection
        internal ObservableCollection<Transaction> transactionEntries;

        //Fallback when sorting
        internal ObservableCollection<Transaction> transactionEntriesFallback;

        public TransactionManager()
        {
            transactionEntries = [];
            transactionEntriesFallback = [];
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

            transactionEntries.Add(newTransaction);
        }

        internal void SortTransactions(Enums.SortBy sortByIn)
        {
            //Instantiate TransactionSorter with sorting instruction
            TransactionSorter transactionSorter = new(sortByIn);

            //Save the collection as fallback
            transactionEntriesFallback = transactionEntries;

            //Clear transactionEntries (that is observed by the UI)
            transactionEntries.Clear();

            //Sort the transactions to a list
            List<Transaction> sortedTransactions = transactionEntries.OrderBy(transaction => transaction, transactionSorter).ToList();

            //Add the transactions to transactionEntries-collection
            foreach (Transaction transaction in sortedTransactions)
            {
                transactionEntries.Add(transaction);
            }
        }
    }
}
