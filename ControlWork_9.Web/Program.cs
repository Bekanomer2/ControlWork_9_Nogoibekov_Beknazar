using System.Globalization;
using ControWork_9.BLL.Services;
using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.DAL.Data;
using ControWork_9.DAL.Repositories;
using ControWork_9.DAL.Repositories.Interfaces;
using ControWork_9.Models;
using ControWork_9.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

[assembly: Microsoft.Extensions.Localization.RootNamespace("ControWork_9")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();
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
builder.Services.AddScoped<IClientServiceAccountRepository, ClientServiceAccountRepository>();
builder.Services.AddScoped<IClientServiceAccountService, ClientServiceAccountService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BalanceContext>();
    if (!await context.Providers.AnyAsync())
    {
        await context.Providers.AddRangeAsync(
            new Provider { Name = "O!", Category = ProvidersCategories.Mobile },
            new Provider { Name = "Beeline", Category = ProvidersCategories.Mobile },
            new Provider { Name = "MegaCom", Category = ProvidersCategories.Mobile },
            new Provider { Name = "Акнет", Category = ProvidersCategories.Internet },
            new Provider { Name = "Кыргызтелеком", Category = ProvidersCategories.Internet },
            new Provider { Name = "Северэлектро", Category = ProvidersCategories.Utilities }
        );
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("ru"),
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

app.UseRequestLocalization(localizationOptions);
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
