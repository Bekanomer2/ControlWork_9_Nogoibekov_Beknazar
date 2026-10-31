namespace ControWork_9.DAL.Repositories.Interfaces;

public interface IBalanceRepository
{
    Task<bool> UpdateBalanceAsync(string accountNumber, decimal amount);
    Task<bool> TransferMoneyAsync(string fromAccountNumber, string toAccountNumber, decimal amount);
    Task<bool> PayForTheServiceAsync(string fromAccountNumber, int providerId, string serviceNumber, decimal amount);
    Task<bool> SaveChangesAsync();
}
