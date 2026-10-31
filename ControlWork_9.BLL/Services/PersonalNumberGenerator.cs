using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.BLL.Services;

public class PersonalNumberGenerator : IPersonalNumberGenerator
{
    private readonly Random random = new();
    private readonly UserManager<User> _userManager;
    private readonly IClientServiceAccountRepository _clientServiceAccountRepository;

    public PersonalNumberGenerator(UserManager<User> userManager, IClientServiceAccountRepository clientServiceAccountRepository)
    {
        _userManager = userManager;
        _clientServiceAccountRepository = clientServiceAccountRepository;
    }

    public async Task<string> GeneratePersonalNumber()
    {
        string accountNumber;
        while (true)
        {
            accountNumber = random.Next(100000, 1000000).ToString();
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
            if (existingUser == null) break;
        }
        return accountNumber;
    }

    public async Task<string> GenerateServiceAccountNumber()
    {
        string accountNumber;
        while (true)
        {
            accountNumber = random.Next(100000, 1000000).ToString();
            var existing = await _clientServiceAccountRepository.AccountNumberExists(accountNumber);
            if (!existing) break;
        }
        return accountNumber;
    }
}
