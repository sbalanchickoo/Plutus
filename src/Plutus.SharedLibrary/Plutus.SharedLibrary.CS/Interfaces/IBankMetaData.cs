using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.SharedLibrary.CS.Interfaces
{
    /// <summary>
    /// Interface for Bank enrichment data from sources
    /// </summary>
    public interface IBankMetadata
    {
        /// <summary>
        /// Returns distinct list of all Bank transactions in repository
        /// </summary>
        IEnumerable<BankMetadata> GetBankMetadata();

        /// <summary>
        /// Returns details of source of repository
        /// </summary>
        InputDataSource GetSourceDetails();
    }
}
