using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementSystem.Models
{
    public class Menuitem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الصنف مطلوب")]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0.01, 10000, ErrorMessage = "السعر لازم يكون أكبر من صفر")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        // Foreign Key: كل صنف بينتمي لتصنيف واحد
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
