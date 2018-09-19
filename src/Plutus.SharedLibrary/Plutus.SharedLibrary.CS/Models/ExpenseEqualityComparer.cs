using System;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// IEquatable methods to compare two Expense objects
    /// </summary>
    public partial class Expense : IEquatable<Expense>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(Expense other)
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
            else if (TransactionCategory != other.TransactionCategory)
            {
                return false;
            }
            else if (Description != other.Description)
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

            // Get the hash code for the Amount field. 
            int hashAmount = Amount.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashTransactionCategory = TransactionCategory.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashDescription = Description.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashDate ^ hashAmount ^ hashTransactionCategory ^ hashDescription;
        }
    }
}
