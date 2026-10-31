using ControWork_9.Models;
using ControWork_9.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ControWork_9.Controllers;

public class MainController : Controller
{
    private readonly UserManager<User> _userManager;

    public MainController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var model = new MainIndexViewModel();
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
