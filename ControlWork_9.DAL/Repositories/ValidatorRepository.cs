using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Repositories;

public class ValidatorRepository : IValidatorRepository
{
    private readonly BalanceContext _context;

    public ValidatorRepository(BalanceContext context)
    {
        _context = context;
    }
    public async Task<bool> CheckEmail(string email, int? id)
    {
        if(id.HasValue)
            return !await _context.Users.AnyAsync(u => u.Email == email &&  u.Id != id.Value);
        return !await _context.Users.AnyAsync(u => u.Email == email);
    }
    
}
