using LibrarySystemAPI.Models;
using LibrarySystemAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPI.Areas.Admin.Controllers
{
    [ApiController]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    public class BookManagementController : ControllerBase
    {
        private readonly IBookRepository _repository;
        public BookManagementController(IBookRepository repository) => _repository = repository;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Book book) 
        {
            await _repository.AddBookAsync(book); 
            
            return Ok(book); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Book book) 
        {
            if (id != book.Id) return BadRequest(); await _repository.UpdateBookAsync(book); 
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            await _repository.DeleteBookAsync(id);
            
            return NoContent(); 
        
        
        }
    }
}