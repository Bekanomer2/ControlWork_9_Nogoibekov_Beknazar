using ControWork_9.Models;

namespace ControWork_9.DAL.Repositories.Interfaces;

public interface IClientServiceAccountRepository
{
    Task CreateClientServiceAccount(ClientServiceAccount clientServiceAccount);
    Task<bool> AccountNumberExists(string accountNumber);
    Task<List<Provider>> GetAllProviders();
    Task<List<ClientServiceAccount>> GetUserAccounts(int userId);
    Task<int> GetTotalProvidersCount();
    Task SaveChangesAsync();
}
