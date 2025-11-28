using BookStoreApp.Dtos;
using BookStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Service
{
    public class StoreStockService
    {


        private readonly BookStoreContext _context;
        private readonly DbService _dbService;

        public StoreStockService(BookStoreContext context, DbService dbService)
        {
            _context = context;
            _dbService = dbService;
        }

        public async Task<List<StoreStockDto>> StoreStock(int storeId)
        {
            var stockRows = await _context.StoreStocks
               .Where(s => s.StoreId == storeId)
               .ToListAsync();

            var books = await _context.Books.ToListAsync();
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.StoreId == storeId);

            if (store == null)
                return new List<StoreStockDto>();

            return stockRows.Select(stock =>
            {
                var book = books.FirstOrDefault(b => b.Isbn13 == stock.Isbn);

                return new StoreStockDto
                {
                    StoreName = store?.StoreName ?? "Unknown Store",
                    BookTitle = book?.Title ?? "Unknown Title",
                    Quantity = stock.Quantity ?? 0,
                    ISBN = stock.Isbn
                };

            }).ToList();
        }

        public async Task AddBook(int storeId, string isbn, int quantity)
        {
            var existingBook = await _context.StoreStocks.FirstOrDefaultAsync(s => s.StoreId == storeId && s.Isbn == isbn);

            if (existingBook != null)
            {
                existingBook.Quantity += quantity;
                await _dbService.Update(existingBook);
            }
            else
            {
                var newBockStock = new StoreStock
                {
                    StoreId = storeId,
                    Isbn = isbn,
                    Quantity = quantity

                };
                await _dbService.Create(newBockStock);
            }
        }

        public async Task<bool> RemoveBook(int storeId, string isbn)
        {

            var existingBook = await _context.StoreStocks.FirstOrDefaultAsync(s => s.StoreId == storeId && s.Isbn == isbn);

            if (existingBook == null)
            {
                return false;
            }
            await _dbService.Delete(existingBook);
            return true;
        }

        public async Task<bool> RemoveBookQuantity(int storeId, string isbn, int quantity)
        {
            var existing = await _context.StoreStocks
                .FirstOrDefaultAsync(s => s.StoreId == storeId && s.Isbn == isbn);

            if (existing == null)
                return false;

            if (existing.Quantity < quantity)
                return false;

            existing.Quantity -= quantity;

            if (existing.Quantity == 0)
                await _dbService.Delete(existing);
            else
                await _dbService.Update(existing);

            return true;
        }

    }
}
