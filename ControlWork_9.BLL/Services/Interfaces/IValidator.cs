namespace ControWork_9.BLL.Services.Interfaces;

public interface IValidator
{
    Task<bool> CheckEmail(string? email, int? id);
    bool CheckPassword(string? password);
}