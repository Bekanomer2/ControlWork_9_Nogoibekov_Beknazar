using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.Models;
using ControWork_9.ViewModels;
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
    private readonly IClientServiceAccountService _clientServiceAccountService;

    public UserController(
        UserManager<User> userManager,
        ITransactionService transactionService,
        IBalanceService balanceService,
        IClientServiceAccountService clientServiceAccountService)
    {
        _userManager = userManager;
        _transactionService = transactionService;
        _balanceService = balanceService;
        _clientServiceAccountService = clientServiceAccountService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        var accounts = await _clientServiceAccountService.GetUserAccounts(user.Id);
        bool isReady = await _clientServiceAccountService.AreAccountsReady(user.Id);

        var model = new UserProfileViewModel
        {
            User = user,
            ServiceAccounts = accounts,
            AreAccountsReady = isReady
        };

        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetServiceAccountsStatus()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { ready = false });

        bool isReady = await _clientServiceAccountService.AreAccountsReady(user.Id);
        return Json(new { ready = isReady });
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> TransferMoney(decimal? amount, string? toPersonalNumber)
    {
        if (string.IsNullOrWhiteSpace(toPersonalNumber))
            return Json(new { success = false, message = "Укажите номер счета получателя" });

        if (!amount.HasValue || amount.Value <= 0)
            return Json(new { success = false, message = "Сумма перевода должна быть больше нуля" });

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return RedirectToAction("Login", "Account");

        if (currentUser.AccountNumber == toPersonalNumber)
            return Json(new { success = false, message = "Нельзя перевести средства на свой собственный счет" });

        if (currentUser.Balance < amount.Value)
            return Json(new { success = false, message = "Недостаточно средств на счете" });

        var targetUser = await _userManager.Users.FirstOrDefaultAsync(x => x.AccountNumber == toPersonalNumber);
        if (targetUser == null)
            return Json(new { success = false, message = "Пользователь с данным личным счетом не найден в системе" });

        bool transferMoneyIsSuccess = await _balanceService.TransferMoneyAsync(currentUser.AccountNumber, toPersonalNumber, amount.Value);
        if (transferMoneyIsSuccess)
            return Json(new { success = true, message = "Перевод успешно выполнен!" });

        return Json(new { success = false, message = "Произошла ошибка при переводе" });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PayForTheServiceAsync(int? providerId, string? serviceNumber, decimal? amount)
    {
        if (!providerId.HasValue || providerId.Value <= 0)
            return Json(new { success = false, message = "Выберите поставщика услуг" });
        if (string.IsNullOrWhiteSpace(serviceNumber))
            return Json(new { success = false, message = "Укажите номер реквизита" });
        if (!amount.HasValue || amount.Value <= 0)
            return Json(new { success = false, message = "Сумма оплаты должна быть больше нуля" });
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return RedirectToAction("Login", "Account");
        if (currentUser.Balance < amount.Value)
            return Json(new { success = false, message = "Недостаточно средств на счете" });
        bool payForTheServiceIsSuccess = await _balanceService.PayForTheServiceAsync(currentUser.AccountNumber, providerId.Value, serviceNumber, amount.Value);
        if (payForTheServiceIsSuccess)
            return Json(new { success = true, message = "Оплата выполнена успешно!" });
        return Json(new { success = false, message = "Реквизит у данного поставщика не найден" });
    }
    

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Transactions(DateTime? fromDate, DateTime? toDate)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");

        ViewBag.AccountNumber = user.AccountNumber;
        ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
        ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

        var list = await _transactionService.GetUserTransactionsAsync(user.AccountNumber, fromDate, toDate);
        return View(list);
    }
}
