using ControWork_9.Models;

namespace ControWork_9.DAL.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task CreateTransactionAsync(Transaction transaction);
}
