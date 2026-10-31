using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.Controllers;

public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ITransactionService _transactionService;
    private readonly IBalanceService _balanceService;

    public UserController(UserManager<User> userManager, ITransactionService transactionService, IBalanceService balanceService)
    {
        _userManager = userManager;
        _transactionService = transactionService;
        _balanceService = balanceService;
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetBalance()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
            return Json(new { isAuthenticated = false, balance = 0 });

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { isAuthenticated = false, balance = 0 });

        return Json(new { isAuthenticated = true, balance = user.Balance });
    }
    
    [HttpPost]
    public async Task<IActionResult> AccountReplenishment(decimal? amount, string? toPersonalNumber)
    {
        if (string.IsNullOrWhiteSpace(toPersonalNumber))
            return Json(new { success = false, message = "Укажите номер личного счета" });

        if (!amount.HasValue || amount.Value <= 0)
            return Json(new { success = false, message = "Сумма пополнения должна быть больше нуля" });

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.AccountNumber == toPersonalNumber);
        if (user == null)
            return Json(new { success = false, message = "Пользователь с данным личным счетом не найден в системе" });

        bool replenishmentIsSuccess = await _balanceService.Replenishment(amount.Value, toPersonalNumber);
        if (replenishmentIsSuccess)
            return Json(new { success = true, message = "Счет успешно пополнен!" });

        return Json(new { success = false, message = "Произошла ошибка при пополнении счета" });
    }
}
