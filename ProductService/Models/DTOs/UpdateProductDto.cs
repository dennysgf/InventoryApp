using System.ComponentModel.DataAnnotations;

namespace ProductService.Models.DTOs
{
    public class UpdateProductDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = "";

        [StringLength(500)]
        public string Description { get; set; } = "";

        [Required, StringLength(50)]
        public string Category { get; set; } = "";

        [Url]
        public string ImageUrl { get; set; } = "";

        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}

