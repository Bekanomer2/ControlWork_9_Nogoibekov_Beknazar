using ControWork_9.Models;

namespace ControWork_9.BLL.Services.Interfaces;

public interface IClientServiceAccountService
{
    Task CreateClientServiceAccounts(int userId);
    Task<bool> AccountNumberExists(string accountNumber);
    Task<List<ClientServiceAccount>> GetUserAccounts(int userId);
    Task<bool> AreAccountsReady(int userId);
    Task<List<Provider>> GetAllProviders();
}
