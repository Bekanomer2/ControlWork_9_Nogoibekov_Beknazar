using Microsoft.AspNetCore.Identity;

namespace ControWork_9.Models;

public class User : IdentityUser<int>
{
   public string AccountNumber { get; set; }
   public string AvatarUrl { get; set; }
   public decimal Balance { get; set; }
}