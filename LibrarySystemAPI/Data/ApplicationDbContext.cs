using LibrarySystemAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;

namespace LibrarySystemAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<LibrarySystemAPI.Models.Order> Orders { get; set; }
        public DbSet<LibrarySystemAPI.Models.OrderItem> OrderItems { get; set; }
    }
}