using BookStoreApp.Dtos;
using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Helpers
{
    public class StoreHelper
    {
        private readonly StoreStockService _ssService;
        private readonly DbService _dbService;
        private readonly BookService _bookService;

        public StoreHelper(StoreStockService ssService, DbService dbService, BookService bookService)
        {
            _ssService = ssService;
            _dbService = dbService;
            _bookService = bookService;
        }
        public async Task<Store?> SelectStore()
        {
            var stores = await _dbService.GetAll<Store>();

            if (stores.Count == 0)
            {
                Console.WriteLine("No stores found.");
                Console.ReadLine();
                return null;
            }

            int? storeId = InputHelper.SelectFromList(
                stores,
                s => $"{s.StoreName} ({s.City})",
                s => s.StoreId
            );

            if (storeId == null)
                return null;

            return stores.First(s => s.StoreId == storeId.Value);
        }

        public async Task<int?> SelectBookFromStore(Store store)
        {

            Console.WriteLine($"=== BOOKS IN {store.StoreName} ({store.City}) ===\n");
            var stock = await _ssService.StoreStock(store.StoreId);

            if (stock.Count == 0)
            {
                Console.WriteLine("Store has no books.");
                Console.ReadLine();
                return null;
            }

            return InputHelper.SelectFromList(
                stock,
                s => $"{s.BookTitle} (Qty: {s.Quantity})",
                s => stock.IndexOf(s)
            );
        }

        public async Task<int?> SelectBookFromList()
        {
            var books = await _bookService.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                Console.ReadLine();
                return null;
            }

            return InputHelper.SelectFromList(
                books,
                b => b.Title,
                b => books.IndexOf(b)
            );
        }
        public int AskQuantity(string message = "Enter quantity:")
        {
            Console.Write(message);

            int qty;

            while (!int.TryParse(Console.ReadLine(), out qty) || qty < 1)
            {
                Console.WriteLine("Invalid number. Try again:");
            }

            return qty;
        }


        public async Task<(Store store, StoreStockDto selectedBook)?> SelectStoreAndBook()
        {
            var store = await SelectStore();
            if (store == null) return null;

            Console.Clear();
            var stock = await _ssService.StoreStock(store.StoreId);
            if (stock.Count == 0)
            {
                Console.WriteLine("Store has no books.");
                Console.ReadLine();
                return null;
            }

            int? index = await SelectBookFromStore(store);
            if (index == null)
            {
                return null;
            }

            return (store, stock[index.Value]);
        }

    }
}
