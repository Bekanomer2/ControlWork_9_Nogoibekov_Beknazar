using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly BalanceContext _context;

    public TransactionRepository(BalanceContext context)
    {
        _context = context;
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<List<Transaction>> GetUserTransactionsAsync(string accountNumber, DateTime? fromDate, DateTime? toDate)
    {
        IQueryable<Transaction> query = _context.Transactions
            .Where(x => x.FromPersonalNumber == accountNumber || x.ToPersonalNumber == accountNumber);
        if (fromDate.HasValue)
        {
            var fromUtc = DateTime.SpecifyKind(fromDate.Value.Date, DateTimeKind.Utc);
            query = query.Where(x => x.Date >= fromUtc);
        }
        if (toDate.HasValue)
        {
            var toUtc = DateTime.SpecifyKind(toDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc);
            query = query.Where(x => x.Date <= toUtc);
        }
        return await query.OrderByDescending(x => x.Date).ToListAsync();
    }
}
