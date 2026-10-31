using System.ComponentModel.DataAnnotations;

namespace ControWork_9.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите email или же личный счет")]
    [Display(Name = "Email или личный счет")]
    public string EmailOrPersonalNumber { get; set; }
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
    [Display(Name = "Запомнить меня")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}