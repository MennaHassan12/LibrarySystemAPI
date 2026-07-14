using LibrarySystemAPI.Data;
using LibrarySystemAPI.DTOs;
using LibrarySystemAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibrarySystemAPI.Areas.Customer.Controllers
{
    [ApiController]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousUser"; 
            var cartItems = await _context.Set<CartItem>()
                .Include(c => c.Book)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return Ok(cartItems);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "AnonymousUser";

            var existingItem = await _context.Set<CartItem>()
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == dto.BookId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    BookId = dto.BookId,
                    Quantity = dto.Quantity
                };
                await _context.Set<CartItem>().AddAsync(cartItem);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Added to cart successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var item = await _context.Set<CartItem>().FindAsync(id);
            if (item == null) return NotFound();

            _context.Set<CartItem>().Remove(item);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Removed from cart!" });
        }
    }
}