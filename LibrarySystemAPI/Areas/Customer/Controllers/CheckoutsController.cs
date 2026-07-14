using LibrarySystemAPI.Data;
using LibrarySystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Climate;
using System.Security.Claims;
using Order = LibrarySystemAPI.Models.Order;

namespace LibrarySystemAPI.Areas.Customer.Controllers
{
    [ApiController]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    public class CheckoutsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CheckoutsController(ApplicationDbContext context) => _context = context;

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousUser";

            var cartItems = await _context.Set<CartItem>()
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
                return BadRequest("Your cart is empty.");

            decimal totalAmount = cartItems.Sum(item => (item.Book?.Price ?? 0) * item.Quantity);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderItems = cartItems.Select(item => new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    Price = item.Book?.Price ?? 0
                }).ToList()
            };

            await _context.Set<Order>().AddAsync(order);

            _context.Set<CartItem>().RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Order placed successfully!", orderId = order.Id, total = totalAmount });
        }
    }
}