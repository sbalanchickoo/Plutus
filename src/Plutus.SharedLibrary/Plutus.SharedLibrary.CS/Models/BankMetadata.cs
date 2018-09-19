using System;

namespace Plutus.SharedLibrary.CS.Models
{
    public partial class BankMetadata
    {
        /// <summary>
        /// Date of transaction
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount of transaction
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Payee, merchant
        /// </summary>
        public string Merchant { get; set; }

        /// <summary>
        /// Accounting category
        /// </summary>
        public string TransactionCategory { get; set; }

        /// <summary>
        /// Additional comments added in Accounting portal
        /// </summary>
        public string UserComments { get; set; }

        
    }
}
