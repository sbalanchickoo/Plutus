using System;

namespace Plutus.SharedLibrary.CS.Models
{
    /// <summary>
    /// IEquatable methods to compare two Invoice objects
    /// </summary>
    public partial class Invoice : IEquatable<Invoice>
    {
        /// <summary>
        /// Overriding equatable method
        /// </summary>
        public bool Equals(Invoice other)
        {
            if (other == null)
            {
                return false;
            }
            else if (Date != other.Date)
            {
                return false;
            }
            else if (ClientName != other.ClientName)
            {
                return false;
            }
            else if (InvoiceReference != other.InvoiceReference)
            {
                return false;
            }
            else if (Amount != other.Amount)
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

            // Get the hash code for the Description field. 
            int hashClientName = ClientName.GetHashCode();

            // Get the hash code for the UserComments field. 
            int hashInvoiceReference = InvoiceReference.GetHashCode();

            // Get the hash code for the Amount field. 
            int hashAmount = Amount.GetHashCode();

            // Get the hash code for the Amount field. 
            int hashDescription = Description.GetHashCode();

            // Calculate the hash code for the transaction. 
            return hashDate ^ hashClientName ^ hashInvoiceReference ^ hashAmount ^ hashDescription;
        }
    }
}
