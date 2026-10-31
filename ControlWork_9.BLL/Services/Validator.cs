using ControWork_9.DAL.Repositories.Interfaces;

namespace ControWork_9.BLL.Services.Interfaces;

public class Validator : IValidator
{
    private readonly IValidatorRepository _repository;

    public Validator(IValidatorRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> CheckEmail(string? email, int? id)
    {
        if (string.IsNullOrWhiteSpace(email))
            return true;
        if(id.HasValue)
            return await _repository.CheckEmail(email, id);
        return await _repository.CheckEmail(email, null);
    }

    public bool CheckPassword(string? password)
    {
        if(string.IsNullOrWhiteSpace(password))
            return true;
        return (password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit));
    }
}