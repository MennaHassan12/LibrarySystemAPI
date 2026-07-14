using LibrarySystemAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemAPI.Areas.Chat.Controllers
{
    [ApiController]
    [Area("Chat")]
    [Route("api/[area]/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ChatController(ApplicationDbContext context) => _context = context;

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var messages = await _context.ChatMessages.OrderBy(m => m.Timestamp).Take(50).ToListAsync();
            
            
            return Ok(messages);
        }
    }
}