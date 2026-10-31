using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.BLL.Services;

public class PersonalNumberGenerator : IPersonalNumberGenerator
{
    private readonly Random random = new();
    private readonly UserManager<User> _userManager;
    public PersonalNumberGenerator(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    public async Task<string> GeneratePersonalNumber()
    {
        string accountNumber;
        while (true)
        {
            accountNumber = random.Next(100000, 1000000).ToString();
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
            if(existingUser == null) break;
        }
        return accountNumber;
    }
}