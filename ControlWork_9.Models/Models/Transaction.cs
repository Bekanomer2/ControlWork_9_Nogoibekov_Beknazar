using System.Transactions;
using ControWork_9.Models.Enums;

namespace ControWork_9.Models;

public class Transaction
{
    public int Id { get; set; }
    public string? FromPersonalNumber { get; set; }
    public string? ToPersonalNumber { get; set; }
    public DateTime Date { get; set; }
    public TransactionTypes Type { get; set; }
    public decimal Amount { get; set; }
}