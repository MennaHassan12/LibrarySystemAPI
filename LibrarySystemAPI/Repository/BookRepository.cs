using LibrarySystemAPI.Data;
using LibrarySystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemAPI.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Book>> GetAllBooksAsync(string? search)
        {
            var query = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(search))
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            return await query.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id) => await _context.Books.FindAsync(id);
        public async Task<IEnumerable<Book>> GetBestSellersAsync() => await _context.Books.Where(b => b.IsBestSeller).ToListAsync();
        public async Task<IEnumerable<Book>> GetRecommendedBooksAsync() => await _context.Books.Where(b => b.IsRecommended).ToListAsync();
        public async Task<IEnumerable<Book>> GetFlashSalesAsync() => await _context.Books.Where(b => b.IsFlashSale).ToListAsync();
        public async Task AddBookAsync(Book book) 
        {
            await _context.Books.AddAsync(book); 
            await _context.SaveChangesAsync(); 
        }
        public async Task UpdateBookAsync(Book book) 
        {
            _context.Books.Update(book); 
            await _context.SaveChangesAsync(); 
        }
        public async Task DeleteBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book != null) { _context.Books.Remove(book); await _context.SaveChangesAsync(); }
        }
    }
}