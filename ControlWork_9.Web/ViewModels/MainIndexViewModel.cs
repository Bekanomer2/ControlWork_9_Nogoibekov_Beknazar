using System.ComponentModel.DataAnnotations;
using ControWork_9.Models;

namespace ControWork_9.ViewModels;

public class MainIndexViewModel
{
    public bool IsAuthenticated { get; set; }
    public User? CurrentUser { get; set; }
    public List<Provider> Providers { get; set; } = new();

    [Required(ErrorMessage = "Укажите номер личного счета")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Номер личного счета должен состоять из 6 цифр")]
    [Display(Name = "Номер личного счета")]
    public string? AccountNumber { get; set; }

    [Required(ErrorMessage = "Укажите сумму пополнения")]
    [Range(1, 100_000_000, ErrorMessage = "Сумма должна быть не менее 1")]
    [Display(Name = "Сумма пополнения")]
    public decimal? Amount { get; set; }
}
