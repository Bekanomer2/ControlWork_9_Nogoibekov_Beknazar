using ControWork_9.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControWork_9.Controllers;

public class ValidationController : Controller
{
    private readonly IValidator _validator;
    public ValidationController(IValidator validator)
    {
        _validator = validator;
    }
    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> CheckEmail(string? email, int? id)
    {
        bool isValid = await _validator.CheckEmail(email, id);
        return Json(isValid);
    }
    [AcceptVerbs("GET", "POST")]
    public IActionResult CheckPassword(string? password)
    {
        bool isValid = _validator.CheckPassword(password);
        return Json(isValid);
    }
}