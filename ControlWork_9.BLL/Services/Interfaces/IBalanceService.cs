namespace ControWork_9.BLL.Services.Interfaces;

public interface IBalanceService
{
    Task<bool> Replenishment(decimal amount, string toPersonalNumber);
    Task<bool> TransferMoneyAsync(string fromAccountNumber, string toAccountNumber, decimal amount);
    Task<bool> PayForTheServiceAsync(string fromAccountNumber, int providerId, string serviceNumber, decimal amount);
}