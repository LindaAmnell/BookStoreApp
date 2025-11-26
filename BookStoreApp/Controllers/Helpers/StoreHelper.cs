using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Helpers
{
    public class StoreHelper
    {

        private readonly StoreStockService _ssService;
        private readonly DbService _dbServce;
        private readonly BookService _bookService;

        public StoreHelper(StoreStockService stockService, DbService dbService, BookService bookService)
        {
            _ssService = stockService;
            _dbServce = dbService;
            _bookService = bookService;
        }
        public async Task ListStoreStock()
        {
            Console.Clear();
            Console.WriteLine("=== LIST STORE STOCK ===\n");

            var stores = await _dbServce.GetAll<Store>();

            if (stores.Count == 0)
            {
                Console.WriteLine("No stores found");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Select a store: \n");

            int? storeId = InputHelper.SelectFromList(stores, s => $"{s.StoreName} ({s.City})",
                s => s.StoreId);

            if (storeId == null)
            {
                Console.WriteLine("\nCancelled");
                Console.ReadLine();
                return;
            }

            var stock = await _ssService.StoreStock(storeId.Value);
            Console.Clear();

            Console.WriteLine($"=== STOCK FOR STORE {stores.First(s => s.StoreId == storeId).StoreName} ===\n");

            if (stock.Count == 0)
            {
                Console.WriteLine("Store has no books in stock");
                Console.ReadLine();
                return;
            }

            foreach (var item in stock)
            {
                Console.WriteLine($"Book: {item.BookTitle} | Qty: {item.Quantity} | ISBN: {item.ISBN}");
            }
            Console.WriteLine("\nPress Enter to return..");
            Console.ReadKey();

        }

        public async Task AddBookToStore()
        {
            Console.Clear();
            Console.WriteLine("=== ADD BOOK TO STORE ===\n");

            var stores = await _dbServce.GetAll<Store>();
            var books = await _bookService.GetAllBooks();

            if (stores.Count == 0 || books.Count == 0)
            {
                Console.WriteLine("no store or books available.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Choose a store:\n");
            int? storeID = InputHelper.SelectFromList(stores, s => s.StoreName, s => s.StoreId);
            if (storeID == null)
            {
                return;
            }

            Console.WriteLine("Choose a book:\n");
            int? bookIndex = InputHelper.SelectFromList(books, b => b.Title, b => books.IndexOf(b));

            if (bookIndex == null)
            {
                Console.WriteLine("\nCancelled");
                Console.ReadLine();
                return;
            }

            var selectedBook = books[bookIndex.Value];

            Console.Write("\nEnter quantity: ");
            int qty;
            while (!int.TryParse(Console.ReadLine(), out qty) || qty < 1)
            {
                Console.WriteLine("Invalid quantity. Enter again: ");
            }

            await _ssService.AddBook(storeID.Value, selectedBook.Isbn13, qty);
            Console.WriteLine("\nBook added to store!");
            Console.ReadKey();
        }

        public async Task RemoveBookFromStore()
        {
            Console.Clear();
            Console.WriteLine("=== REMOVE BOOK FROM STORE === ");

            var stores = await _dbServce.GetAll<Store>();

            if (stores.Count == 0)
            {
                Console.WriteLine("No stores found");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Choose a store:\n");
            int? storeId = InputHelper.SelectFromList(stores, s => $"{s.StoreName} ({s.City})",
            s => s.StoreId);


            if (storeId == null)
            {
                Console.WriteLine("Cancelled");
                Console.ReadLine();
                return;
            }

            var stock = await _ssService.StoreStock(storeId.Value);

            if (stock.Count == 0)
            {
                Console.WriteLine("Store has no books");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("\nChoose a book to remove:\n");

            int? index = InputHelper.SelectFromList(stock, s => $"{s.BookTitle} (Qty: {s.Quantity})", s => stock.IndexOf(s));

            if (index == null)
            {
                Console.WriteLine("Cancelled");
                Console.ReadLine();
                return;
            }

            var selected = stock[index.Value];

            bool success = await _ssService.RemoveBook(storeId.Value, selected.ISBN);

            Console.WriteLine(success ? "\nBook remved from store!" : "\nFailed to remove book.");

            Console.ReadLine();


        }

    }




}
