using ControWork_9.BLL.Services.Interfaces;
using ControWork_9.Models;
using ControWork_9.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControWork_9.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IWebHostEnvironment _environment;
    private readonly IValidator _validator;
    private readonly IPersonalNumberGenerator _personalNumberGenerator;
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment,  IValidator validator,  IPersonalNumberGenerator personalNumberGenerator)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _environment = environment;
        _validator = validator;
        _personalNumberGenerator = personalNumberGenerator;
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.Email) && !await _validator.CheckEmail(model.Email, null))
            ModelState.AddModelError(nameof(model.Email), "В базе уже существует аккаунт с данным адресом почты");
        if (!string.IsNullOrWhiteSpace(model.Password) && !_validator.CheckPassword(model.Password))
            ModelState.AddModelError(nameof(model.Password), "Пароль должен содержать в себе 1 букву верхнего регистра, 1 букву нижнего регистра, а также минимум 1 цифру");
        string avatarPath = "/default.jpg";
        if (ModelState.IsValid)
        {
            if (model.Avatar != null)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "avatars");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(model.Avatar.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await model.Avatar.CopyToAsync(fileStream);
                avatarPath = $"/uploads/avatars/{uniqueFileName}";   
            }
            User userToCreate = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                AvatarUrl = avatarPath,
                Balance = 100000,
                AccountNumber = await _personalNumberGenerator.GeneratePersonalNumber()
            }; 
            var result = await _userManager.CreateAsync(userToCreate, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(userToCreate, isPersistent: false);
                return RedirectToAction("Index", "Main");
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
    
    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Main");
        }
        
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User? user = await _userManager.FindByEmailAsync(model.EmailOrPersonalNumber);
            if (user == null)
                user = await _userManager.Users.FirstOrDefaultAsync(x => x.AccountNumber == model.EmailOrPersonalNumber);
            if (user != null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false
                );
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Main");
                }
            }
            ModelState.AddModelError("", "Неправильный логин и (или) пароль");
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOffAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}