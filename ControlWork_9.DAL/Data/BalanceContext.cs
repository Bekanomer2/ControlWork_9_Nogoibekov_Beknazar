using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.DAL.Data;

public class BalanceContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<ClientServiceAccount> ClientServiceAccounts { get; set; }
    public BalanceContext(DbContextOptions<BalanceContext> options) : base(options){}
}