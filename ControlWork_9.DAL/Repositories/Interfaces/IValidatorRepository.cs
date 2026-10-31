namespace ControWork_9.DAL.Repositories.Interfaces;

public interface IValidatorRepository
{
    Task<bool> CheckEmail(string email, int? id);
}