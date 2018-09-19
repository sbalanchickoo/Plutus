using System;

namespace Plutus.SharedLibrary.CS.Models
{
    public partial class Expense
    {
        /// <summary>
        /// Date of expense
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Amount of expense
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Accounting category
        /// </summary>
        public string TransactionCategory { get; set; }

        /// <summary>
        /// Additional comments added in Accounting portal
        /// </summary>
        public string Description { get; set; }
    }
}
