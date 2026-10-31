namespace ControWork_9.DAL.Repositories.Interfaces;

public interface IBalanceRepository
{
    Task<bool> UpdateBalanceAsync(string accountNumber, decimal amount);
    Task<bool> SaveChangesAsync();
}
