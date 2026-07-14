using LibrarySystemAPI.Models;

namespace LibrarySystemAPI.Repository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync(string? search);
        Task<Book?> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetBestSellersAsync();
        Task<IEnumerable<Book>> GetRecommendedBooksAsync();
        Task<IEnumerable<Book>> GetFlashSalesAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);
    }
}