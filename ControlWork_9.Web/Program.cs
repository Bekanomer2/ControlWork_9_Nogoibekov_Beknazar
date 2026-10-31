using ControWork_9.BLL.Services;
using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BalanceContext>(options => options.UseNpgsql(connection))
    .AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯөүңӨҮҢ";
    })
    .AddEntityFrameworkStores<BalanceContext>();
builder.Services.AddScoped<IValidatorRepository, ValidatorRepository>();
builder.Services.AddScoped<IValidator, Validator>();
builder.Services.AddScoped<IPersonalNumberGenerator, PersonalNumberGenerator>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Main}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
