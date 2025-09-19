using System;

namespace TransactionService.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string Type { get; set; } = ""; 
    public int ProductId { get; set; }     
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public string Detail { get; set; } = "";
}