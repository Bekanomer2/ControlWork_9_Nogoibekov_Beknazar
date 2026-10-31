using ControWork_9.Models.Enums;

namespace ControWork_9.Models;

public class Provider
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProvidersCategories Category { get; set; }
    public ICollection<ClientServiceAccount> ClientAccounts { get; set; } = new List<ClientServiceAccount>();
}