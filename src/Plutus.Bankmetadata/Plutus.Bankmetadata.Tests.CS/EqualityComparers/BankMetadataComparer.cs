using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.Bankmetadata.Tests.CS.EqualityComparers
{
    internal class BankMetadataComparer : Comparer<BankMetadata>
    {
        /// <summary>
        /// Returns 0 if all attributes of two BankMetadata records match 
        /// </summary>
        public override int Compare(BankMetadata x, BankMetadata y)
        {
            if (x.Date.CompareTo(y.Date) == 0 
                && x.Amount.CompareTo(y.Amount) == 0 
                && x.TransactionCategory.CompareTo(y.TransactionCategory) == 0 
                && x.Merchant.CompareTo(y.Merchant) == 0
                && x.UserComments.CompareTo(y.UserComments) == 0)
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

