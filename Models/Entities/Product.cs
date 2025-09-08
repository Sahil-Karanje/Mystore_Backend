using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string LongDescription { get; set; }

        [MaxLength(250)]
        public string ShortDescription { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }  

        [Required]
        public int Stock { get; set; }

        [MaxLength(100)]
        public string Brand { get; set; }

        [MaxLength(100)]
        public string Category { get; set; }

        [MaxLength(255)]
        public string ImageUrl { get; set; }  

        public bool IsActive { get; set; } = true; 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
