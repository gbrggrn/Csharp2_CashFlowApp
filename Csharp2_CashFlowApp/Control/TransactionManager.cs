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
    /// <summary>
    /// Responsible for managing transactions.
    /// </summary>
    [Serializable]
    public class TransactionManager
    {
        //Transaction-collection
        public ObservableCollection<Transaction> TransactionEntries { get; set; }

        /// <summary>
        /// Constructor initializes.
        /// </summary>
        public TransactionManager()
        {
            TransactionEntries = [];
        }

        /// <summary>
        /// Extracts and adds transaction data from a DTO to a transaction.
        /// </summary>
        /// <param name="transactionDTO">The DTO to be extracted from</param>
        internal void AddTransaction(TransactionDTO transactionDTO)
        {
            //Extract and assign category
            Category newCategory = new()
            {
                CategoryName = transactionDTO.CategoryNameTransfer,
                CategoryType = transactionDTO.CategoryTypeTransfer
            };

            //Extract and assign transaction
            Transaction newTransaction = new()
            {
                Date = transactionDTO.DateTimeTransfer,
                Amount = transactionDTO.AmountTransfer,
                Category = newCategory,
                Description = transactionDTO.DescriptionTransfer
            };

            //Add to transactionentries
            TransactionEntries.Add(newTransaction);
        }
    }
}
