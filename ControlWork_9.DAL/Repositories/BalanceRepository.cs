using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Repositories;

public class BalanceRepository : IBalanceRepository
{
    private readonly BalanceContext _context;

    public BalanceRepository(BalanceContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateBalanceAsync(string accountNumber, decimal amount)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        if (user == null)
            return false;

        user.Balance += amount;
        return true;
    }

    public async Task<bool> TransferMoneyAsync(string fromAccountNumber, string toAccountNumber, decimal amount)
    {
        var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.AccountNumber == fromAccountNumber);
        var user = await _context.Users.FirstOrDefaultAsync(x => x.AccountNumber == toAccountNumber);
        if (currentUser == null || user == null || currentUser.Balance < amount)
            return false;
        currentUser.Balance -= amount;
        user.Balance += amount;
        return true;
    }

    public async Task<bool> SaveChangesAsync()
    {
        int savedRows = await _context.SaveChangesAsync();
        return savedRows > 0;
    }
}
