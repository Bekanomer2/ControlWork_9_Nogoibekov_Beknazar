using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;

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
}
