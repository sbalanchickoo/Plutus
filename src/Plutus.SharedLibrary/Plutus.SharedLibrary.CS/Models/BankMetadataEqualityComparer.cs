using System;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// IEquatable methods to compare two objects in list
    /// </summary>
    public partial class BankMetadata : IEquatable<BankMetadata>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(BankMetadata other)
        {
            if (other == null)
            {
                return false;
            }
            else if (Date != other.Date)
            {
                return false;
            }
            else if (Amount != other.Amount)
            {
                return false;
            }
            else if (Merchant != other.Merchant)
            {
                return false;
            }
            else if (UserComments != other.UserComments)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Hash code for the Metadata for equality checks
        /// </summary>
        public override int GetHashCode()
        {
            // Get the hash code for the Date field if it is not null. 
            int hashDate = Date == null ? 0 : Date.GetHashCode();

            // Get the hash code for the Description field. 
            int hashMerchant = Merchant.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashUserComments = UserComments.GetHashCode();

            // Get the hash code for the Amount field. 
            int hashAmount = Amount.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashDate ^ hashMerchant ^ hashUserComments ^ hashAmount;
        }
    }
}
