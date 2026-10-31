using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;

namespace ControWork_9.BLL.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {
        await _transactionRepository.CreateTransactionAsync(transaction);
    }
}
