using ControWork_9.Models;

namespace ControWork_9.BLL.Services.Interfaces;

public interface ITransactionService
{
    Task CreateTransactionAsync(Transaction transaction);
}
