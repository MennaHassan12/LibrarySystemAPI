namespace LibrarySystemAPI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public bool IsBestSeller { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsFlashSale { get; set; }
        public decimal? FlashSalePrice { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }
        public int? FlashSaleStockLeft { get; set; }
    }
}