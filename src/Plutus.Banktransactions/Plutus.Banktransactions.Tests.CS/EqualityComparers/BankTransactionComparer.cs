using Plutus.SharedLibrary.CS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Plutus.Banktransactions.Tests.CS.EqualityComparers
{
    internal class BankTransactionComparer : Comparer<BankTransaction>
    {
        /// <summary>
        /// Returns 0 if all attributes (FITID, Merchant, Amount, Date) of two BankTransaction records match 
        /// </summary>
        public override int Compare(BankTransaction x, BankTransaction y)
        {
            if (x.FITID.CompareTo(y.FITID) == 0 && x.Amount.CompareTo(y.Amount) == 0 && x.Date.CompareTo(y.Date) == 0 && x.Merchant.CompareTo(y.Merchant) == 0)
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

