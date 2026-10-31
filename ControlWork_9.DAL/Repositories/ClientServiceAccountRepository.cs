using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Repositories;

public class ClientServiceAccountRepository : IClientServiceAccountRepository
{
    private readonly BalanceContext _context;

    public ClientServiceAccountRepository(BalanceContext context)
    {
        _context = context;
    }

    public async Task CreateClientServiceAccount(ClientServiceAccount clientServiceAccount)
    { 
        await _context.ClientServiceAccounts.AddAsync(clientServiceAccount);
    }

    public async Task<bool> AccountNumberExists(string accountNumber)
    {
        return await _context.ClientServiceAccounts.AnyAsync(c => c.AccountNumber == accountNumber);
    }

    public async Task<List<Provider>> GetAllProviders()
    {
        return await _context.Providers.ToListAsync();
    }

    public async Task<List<ClientServiceAccount>> GetUserAccounts(int userId)
    {
        return await _context.ClientServiceAccounts
            .Include(c => c.Provider)
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> GetTotalProvidersCount()
    {
        return await _context.Providers.CountAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
