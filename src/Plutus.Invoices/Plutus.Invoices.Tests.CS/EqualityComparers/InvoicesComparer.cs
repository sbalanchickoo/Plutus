using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.Invoices.Tests.CS.EqualityComparers
{
    internal class InvoicesComparer : Comparer<Invoice>
    {
        /// <summary>
        /// Returns 0 if all attributes of two Invoice records match 
        /// </summary>
        public override int Compare(Invoice x, Invoice y)
        {
            if (x.Date.CompareTo(y.Date) == 0 
                && x.Amount.CompareTo(y.Amount) == 0 
                && x.ClientName.CompareTo(y.ClientName) == 0 
                && x.InvoiceReference.CompareTo(y.InvoiceReference) == 0
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

