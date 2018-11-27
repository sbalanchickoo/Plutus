namespace Plutus.UI.Shared.CS.Models
{
    public class Merchant
    {
        public int MerchantId { get; set; }

        public string MerchantName { get; set; }

        public MasterMerchant MasterMerchant { get; set; }

        public int MasterMerchantId { get; set; }
    }
}
