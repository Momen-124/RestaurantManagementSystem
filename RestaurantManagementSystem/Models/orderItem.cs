namespace RestaurantManagementSystem.Models
{
    public class orderItem
    {

        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int MenuItemId { get; set; }
        public Menuitem? MenuItem { get; set; }

        public int Quantity { get; set; }
    }
}
