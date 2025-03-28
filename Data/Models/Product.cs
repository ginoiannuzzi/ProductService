using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProductService.Data.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid SubcategoryId { get; set; }  // Foreign Key

        [ForeignKey(nameof(SubcategoryId))]
        [JsonIgnore]
        public Subcategory Subcategory { get; set; } = null!;

        [Required, MaxLength(15)]
        public string Ski { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
