using System;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// IEquatable methods to compare two objects in list
    /// </summary>
    public partial class BankTransaction : IEquatable<BankTransaction>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(BankTransaction other)
        {
            if (other == null) { 
                return false;
            }
            else if (FITID != other.FITID) { 
                return false;
            }
            else { 
                return true;
            }
        }

        /// <summary>
        /// Hash code for the transaction for equality checks
        /// </summary>
        public override int GetHashCode()
        {
            // Get the hash code for the Date field if it is not null. 
            int hashFITID = FITID == Convert.ToString(null) ? 0 : FITID.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashFITID;
        }
    }
}
