using Plutus.SharedLibrary.CS.Interfaces;

namespace Plutus.Banktransaction.Service.API.Interfaces
{
    /// <summary>
    /// Repository Interface for BankTransaction
    /// </summary>
    public interface IBankTransactionRepo
    {
        /// <summary>
        /// Returns a fully instantiated repository
        /// </summary>
        IBankTransaction GetRepo();
    }
}
