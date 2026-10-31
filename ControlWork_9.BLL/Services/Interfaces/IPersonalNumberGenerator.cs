namespace ControWork_9.BLL.Services.Interfaces;

public interface IPersonalNumberGenerator
{
    Task<string> GeneratePersonalNumber();
    Task<string> GenerateServiceAccountNumber();
}