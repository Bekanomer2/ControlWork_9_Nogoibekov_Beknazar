using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using ControWork_9.Models.Enums;

namespace ControWork_9.BLL.Services;

public class BalanceService : IBalanceService
{
    private readonly IBalanceRepository _balanceRepository;
    private readonly ITransactionRepository _transactionRepository;

    public BalanceService(IBalanceRepository balanceRepository, ITransactionRepository transactionRepository)
    {
        _balanceRepository = balanceRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<bool> Replenishment(decimal amount, string toPersonalNumber)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(toPersonalNumber))
            return false;
        bool balanceUpdated = await _balanceRepository.UpdateBalanceAsync(toPersonalNumber, amount);
        if (!balanceUpdated)
            return false;
        var transaction = new Transaction
        {
            Amount = amount,
            ToPersonalNumber = toPersonalNumber,
            Date = DateTime.UtcNow,
            Type = TransactionTypes.Пополнение
        };

        await _transactionRepository.CreateTransactionAsync(transaction);

        return await _balanceRepository.SaveChangesAsync();
    }

    public async Task<bool> TransferMoneyAsync(string fromAccountNumber, string toAccountNumber, decimal amount)
    {
        if (amount <= 0 || string.IsNullOrWhiteSpace(toAccountNumber) || string.IsNullOrWhiteSpace(fromAccountNumber))
            return false;
        bool transferIsSuccess = await _balanceRepository.TransferMoneyAsync(fromAccountNumber, toAccountNumber, amount);
        if(!transferIsSuccess)
            return false;
        var transaction = new Transaction
        {
            Amount = amount,
            FromPersonalNumber =  fromAccountNumber,
            ToPersonalNumber = toAccountNumber,
            Date = DateTime.UtcNow,
            Type = TransactionTypes.Перевод
        };
        await _transactionRepository.CreateTransactionAsync(transaction);
        return await _balanceRepository.SaveChangesAsync();
    }
}
