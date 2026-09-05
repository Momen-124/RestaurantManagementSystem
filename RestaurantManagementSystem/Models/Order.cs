using System.ComponentModel.DataAnnotations;

namespace RestaurantManagementSystem.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Cancelled

        public string? CustomerName { get; set; }

        public List<orderItem> OrderItems { get; set; } = new List<orderItem>();
    }
}
