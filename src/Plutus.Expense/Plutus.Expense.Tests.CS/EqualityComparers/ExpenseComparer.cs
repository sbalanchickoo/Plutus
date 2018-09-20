using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.Expenses.Tests.CS.EqualityComparers
{
    internal class ExpenseComparer : Comparer<Expense>
    {
        /// <summary>
        /// Returns 0 if all attributes of two BankMetadata records match 
        /// </summary>
        public override int Compare(Expense x, Expense y)
        {
            if (x.Date.CompareTo(y.Date) == 0 
                && x.Amount.CompareTo(y.Amount) == 0 
                && x.TransactionCategory.CompareTo(y.TransactionCategory) == 0 
                && x.Description.CompareTo(y.Description) == 0)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}

