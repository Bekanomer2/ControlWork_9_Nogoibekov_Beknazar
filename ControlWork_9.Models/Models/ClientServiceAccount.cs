namespace ControWork_9.Models;

public class ClientServiceAccount
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int ProviderId { get; set; }
    public Provider? Provider { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
}