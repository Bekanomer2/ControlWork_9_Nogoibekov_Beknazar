using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.Models;
using ControWork_9.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControWork_9.Controllers;

public class MainController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IClientServiceAccountService _clientServiceAccountService;

    public MainController(UserManager<User> userManager, IClientServiceAccountService clientServiceAccountService)
    {
        _userManager = userManager;
        _clientServiceAccountService = clientServiceAccountService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new MainIndexViewModel();
        model.Providers = await _clientServiceAccountService.GetAllProviders();

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                model.IsAuthenticated = true;
                model.CurrentUser = user;
            }
        }
        return View(model);
    }
}
