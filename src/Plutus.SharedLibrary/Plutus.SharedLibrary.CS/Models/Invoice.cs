using System;

namespace Plutus.SharedLibrary.CS.Models
{
    public partial class Invoice
    {
        /// <summary>
        /// Date of invoice
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// ClientName
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Invoice reference
        /// </summary>
        public string InvoiceReference { get; set; }

        /// <summary>
        /// Amount of invoice
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Additional comments added in Accounting portal
        /// </summary>
        public string Description { get; set; }
    }
}
