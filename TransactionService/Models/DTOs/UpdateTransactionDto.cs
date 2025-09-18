using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models.DTOs
{
    public class UpdateTransactionDto
    {
        [Required]
        public string Type { get; set; } = "";

        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.01, 999999.99)]
        public decimal UnitPrice { get; set; }

        [StringLength(500)]
        public string Detail { get; set; } = "";
    }
}

