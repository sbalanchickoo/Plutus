using System;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// Row from business bank. Class is defined as partial to allow for equality methods to be written
    /// </summary>
    public partial class BankTransaction
    {
        /// <summary>
        /// Unique identifier supplied by bank
        /// </summary>
        public string FITID { get; set; }

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
    }
}
