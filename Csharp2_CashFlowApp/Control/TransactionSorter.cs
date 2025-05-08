using Csharp2_CashFlowApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csharp2_CashFlowApp.Control
{
    /// <summary>
    /// Comparer logic for sorting methods.
    /// </summary>
    internal class TransactionSorter : IComparer<Transaction>
    {
        private readonly Enums.SortBy sortBy;

        /// <summary>
        /// Constructor initializes instance variable.
        /// </summary>
        /// <param name="sortByIn">The chosen enum SortBy</param>
        public TransactionSorter(Enums.SortBy sortByIn)
        {
            sortBy = sortByIn;
        }

        /// <summary>
        /// Defines the comparer logic for sorting animals either by Name or Species.
        /// </summary>
        /// <param name="x">The first animal</param>
        /// <param name="y">The second animal</param>
        /// <returns>
        /// Negative value if "x" comes before "y"
        /// Positive value if "x" comes after "y"
        /// Zero (0) if the params are equal (default case)
        /// </returns>
        /// <exception cref="ArgumentException">Throws if either param is null</exception>
        public int Compare(Transaction? x, Transaction? y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Can not sort null values");
            }

            return sortBy switch
            {
                Enums.SortBy.Month => string.Compare(
                    x.Date.Month.ToString(),
                    y.Date.Month.ToString(),
                    StringComparison.Ordinal),
                Enums.SortBy.CategoryName => string.Compare(
                    x.Category!.CategoryName.ToString(),
                    y.Category!.CategoryName.ToString(),
                    StringComparison.Ordinal),
                Enums.SortBy.CategoryType => string.Compare(
                    x.Category!.CategoryType.ToString(),
                    y.Category!.CategoryType.ToString(),
                    StringComparison.Ordinal),
                Enums.SortBy.Amount => string.Compare(
                    x.Amount.ToString(),
                    y.Amount.ToString(),
                    StringComparison.Ordinal),
                _ => 0,
            };
        }
    }
}
