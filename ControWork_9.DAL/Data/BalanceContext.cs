using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Data;

public class BalanceContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    DbSet<User> Users { get; set; }
    
    public BalanceContext(DbContextOptions<BalanceContext> options) : base(options){}
}