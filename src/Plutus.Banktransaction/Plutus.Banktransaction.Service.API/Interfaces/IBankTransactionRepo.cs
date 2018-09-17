using Plutus.SharedLibrary.CS.Interfaces;

namespace Plutus.Banktransaction.Service.API.Interfaces
{
    public interface IBankTransactionRepo
    {
        IBankTransaction GetRepo();
    }
}
