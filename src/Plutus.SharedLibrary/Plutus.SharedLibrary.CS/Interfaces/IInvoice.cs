using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.SharedLibrary.CS.Interfaces
{
    /// <summary>
    /// Interface for Invoice data from sources
    /// </summary>
    public interface IInvoice
    {
        /// <summary>
        /// Returns distinct list of all Invoices in repository
        /// </summary>
        IEnumerable<Invoice> GetInvoices();

        /// <summary>
        /// Returns details of source of repository
        /// </summary>
        InputDataSource GetSourceDetails();
    }
}
