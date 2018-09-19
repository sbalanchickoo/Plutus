namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// Class denoting type of Transaction, such as 'Cash received from clients' etc.
    /// </summary>
    public class TransactionCategory
    {
        /// <summary>
        /// Transaction Category Unique Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Details of Transaction Category
        /// </summary>
        public string TransactionCategoryName { get; set; }

        /// <summary>
        /// If an expense, whether it is Corporation Tax deductible
        /// </summary>
        public bool CorpTaxDeductible { get; set; }
    }
}
