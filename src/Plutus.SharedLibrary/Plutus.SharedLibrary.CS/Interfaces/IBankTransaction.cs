using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.SharedLibrary.CS.Interfaces
{
    /// <summary>
    /// Interface for Business Bank transactions from sources
    /// </summary>
    public interface IBankTransaction
    {
        /// <summary>
        /// Returns distinct list of all Bank transactions in repository
        /// </summary>
        IEnumerable<BankTransaction> GetBankTransactions();

        /// <summary>
        /// Returns details of source of repository
        /// </summary>
        InputDataSource GetSourceDetails();
    }
}
