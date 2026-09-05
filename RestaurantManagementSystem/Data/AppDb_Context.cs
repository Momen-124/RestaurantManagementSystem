using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Models; // غيّر الاسم لاسم مشروعك

namespace RestaurantManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Menuitem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<orderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }
    }
}