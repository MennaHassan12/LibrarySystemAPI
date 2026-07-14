using LibrarySystemAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPI.Areas.Customer.Controllers
{
    [ApiController]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;
        public BooksController(IBookRepository repository) => _repository = repository;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search) => Ok(await _repository.GetAllBooksAsync(search));

        [HttpGet("best-sellers")]
        public async Task<IActionResult> GetBestSellers() => Ok(await _repository.GetBestSellersAsync());

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended() => Ok(await _repository.GetRecommendedBooksAsync());

        [HttpGet("flash-sales")]
        public async Task<IActionResult> GetFlashSales() => Ok(await _repository.GetFlashSalesAsync());
    }
}