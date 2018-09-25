using System;
using System.Collections.Generic;
using System.Text;

namespace Plutus.UI.Shared.CS.Models
{
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }

        public string TransactionTypeName { get; set; }

        public int TransactionCategoryId { get; set; }

        public TransactionCategory TransactionCategory { get; set; }
    }
}
