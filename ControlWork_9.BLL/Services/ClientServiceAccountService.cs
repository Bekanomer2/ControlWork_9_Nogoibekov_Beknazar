using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using ControWork_9.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace ControWork_9.BLL.Services;

public class ClientServiceAccountService : IClientServiceAccountService
{
    private readonly IClientServiceAccountRepository _clientServiceAccountRepository;
    private readonly IPersonalNumberGenerator _personalNumberGenerator;
    private readonly UserManager<User> _userManager;

    public ClientServiceAccountService(
        IClientServiceAccountRepository clientServiceAccountRepository,
        IPersonalNumberGenerator personalNumberGenerator,
        UserManager<User> userManager)
    {
        _clientServiceAccountRepository = clientServiceAccountRepository;
        _personalNumberGenerator = personalNumberGenerator;
        _userManager = userManager;
    }

    public async Task CreateClientServiceAccounts(int userId)
    {
        var providers = await _clientServiceAccountRepository.GetAllProviders();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return;

        foreach (var provider in providers)
        {
            string accountNumber;
            if (provider.Category == ProvidersCategories.Mobile)
            {
                accountNumber = user.PhoneNumber ?? await _personalNumberGenerator.GenerateServiceAccountNumber();
            } 
            else
            {
                accountNumber = await _personalNumberGenerator.GenerateServiceAccountNumber();
            }

            ClientServiceAccount clientServiceAccount = new ClientServiceAccount
            {
                AccountNumber = accountNumber,
                UserId = userId,
                ProviderId = provider.Id,
                Balance = 0
            };
            await _clientServiceAccountRepository.CreateClientServiceAccount(clientServiceAccount);
        }
        await _clientServiceAccountRepository.SaveChangesAsync();
    }

    public async Task<bool> AccountNumberExists(string accountNumber)
    {
        return await _clientServiceAccountRepository.AccountNumberExists(accountNumber);
    }

    public async Task<List<ClientServiceAccount>> GetUserAccounts(int userId)
    {
        return await _clientServiceAccountRepository.GetUserAccounts(userId);
    }

    public async Task<bool> AreAccountsReady(int userId)
    {
        var accounts = await _clientServiceAccountRepository.GetUserAccounts(userId);
        var totalProviders = await _clientServiceAccountRepository.GetTotalProvidersCount();
        return totalProviders > 0 && accounts.Count >= totalProviders;
    }

    public async Task<List<Provider>> GetAllProviders()
    {
        return await _clientServiceAccountRepository.GetAllProviders();
    }
}
