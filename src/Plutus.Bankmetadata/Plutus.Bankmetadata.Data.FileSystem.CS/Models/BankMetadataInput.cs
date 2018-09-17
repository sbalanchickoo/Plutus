using System;

namespace Plutus.Bankmetadata.Data.FileSystem.CS.Models
{
    public class BankMetadataInput
    {
        public DateTime date { get; set; }
        public string bankdescription { get; set; }
        public string accountnumber { get; set; }
        public string employeename { get; set; }
        public string description { get; set; }
        public string usercomments { get; set; }
        public decimal amount { get; set; }
        public decimal net { get; set; }
        public decimal vat { get; set; }
    }
}
