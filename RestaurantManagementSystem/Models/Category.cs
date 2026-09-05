using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [StringLength(50)]
        public string Name { get; set; }

        // علاقة: تصنيف واحد له أصناف كتير
        public List<Menuitem>? MenuItems { get; set; }
    }
}
