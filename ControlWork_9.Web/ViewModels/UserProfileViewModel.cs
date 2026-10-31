using ControWork_9.Models;

namespace ControWork_9.ViewModels;

public class UserProfileViewModel
{
    public User User { get; set; }
    public List<ClientServiceAccount> ServiceAccounts { get; set; } = new();
    public bool AreAccountsReady { get; set; }
}
