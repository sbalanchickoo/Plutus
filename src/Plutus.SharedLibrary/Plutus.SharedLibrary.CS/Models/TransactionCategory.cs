namespace Plutus.SharedLibrary.CS.Models
{
    public class TransactionCategory
    {
        public int Id { get; set; }

        public string TransactionCategoryName { get; set; }

        public bool CorpTaxDeductible { get; set; }
    }
}
