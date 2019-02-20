using System.ComponentModel.DataAnnotations;

namespace RetailApi.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(minimum: 0.01, maximum: (double) decimal.MaxValue)]
        public decimal Price { get; set; }
    }
}