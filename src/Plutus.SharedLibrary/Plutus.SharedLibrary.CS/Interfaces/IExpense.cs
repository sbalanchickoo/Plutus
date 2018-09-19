using Plutus.SharedLibrary.CS.Models;
using System.Collections.Generic;

namespace Plutus.SharedLibrary.CS.Interfaces
{
    /// <summary>
    /// Interface forExpense data from sources
    /// </summary>
    public interface IExpense
    {
        /// <summary>
        /// Returns distinct list of all Expenses in repository
        /// </summary>
        IEnumerable<Expense> GetExpenses();

        /// <summary>
        /// Returns details of source of repository
        /// </summary>
        InputDataSource GetSourceDetails();
    }
}
