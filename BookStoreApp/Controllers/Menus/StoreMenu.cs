using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class StoreMenu
    {
        private readonly StoreHelper _storeHelper;
        private readonly StoreStockService _storeStockService;
        private readonly BookService _bookService;

        public StoreMenu(StoreStockService storeStockService, DbService dbService, BookService bookService)
        {
            _storeStockService = storeStockService;
            _bookService = bookService;

            _storeHelper = new StoreHelper(storeStockService, dbService, bookService);
        }

        public async Task StoreMenuScreen()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== STORE MENU ===\n");
                Console.WriteLine("1. List store stock");
                Console.WriteLine("2. Add book to store");
                Console.WriteLine("3. Remove book from store");
                Console.WriteLine("0. Back\n");

                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ListStoreStock();
                        break;

                    case "2":
                        await AddBookToStore();
                        break;

                    case "3":
                        await RemoveBookFromStore();
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Press Enter...");
                        Console.ReadLine();
                        break;
                }
            }
        }
        private async Task ListStoreStock()
        {
            Console.Clear();
            Console.WriteLine("=== STORE STOCK ===\n");

            var store = await _storeHelper.SelectStore();
            if (store == null)
            {
                return;
            }
            Console.Clear();
            Console.WriteLine($"Store: {store.StoreName} ({store.City})\n");

            var stock = await _storeStockService.StoreStock(store.StoreId);

            if (stock.Count == 0)
            {
                Console.WriteLine("Store has no books.");
                Console.ReadLine();
                return;
            }

            foreach (var item in stock)
            {
                Console.WriteLine($"{item.BookTitle} | Qty: {item.Quantity}");
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }

        private async Task AddBookToStore()
        {
            Console.Clear();
            Console.WriteLine("=== ADD BOOK TO STORE ===\n");

            var store = await _storeHelper.SelectStore();
            if (store == null)
            {
                return;
            }
            Console.Clear();

            Console.WriteLine($"=== BOOKS IN {store.StoreName} ({store.City}) ===\n");
            int? bookIndex = await _storeHelper.SelectBookFromList();
            if (bookIndex == null) return;

            var books = await _bookService.GetAllBooks();
            var selectedBook = books[bookIndex.Value];

            int qty = _storeHelper.AskQuantity("\nEnter quantity to add: ");

            await _storeStockService.AddBook(store.StoreId, selectedBook.Isbn13, qty);

            Console.WriteLine("\nBook added!");
            Console.ReadLine();
        }


        private async Task RemoveBookFromStore()
        {
            Console.Clear();
            Console.WriteLine("=== REMOVE BOOK FROM STORE ===\n");

            var result = await _storeHelper.SelectStoreAndBook();
            if (result == null)
            {
                return;
            }
            var (store, selected) = result.Value;

            if (InputHelper.Confirm($"Delete ALL of {selected.BookTitle}? (y/n):"))
            {
                await _storeStockService.RemoveBook(store.StoreId, selected.ISBN);
                Console.WriteLine("\nBook removed from store!");
                Console.ReadLine();
                return;
            }

            int qty = _storeHelper.AskQuantity($"\nHow many to delete (1–{selected.Quantity}) \n");
            qty = Math.Min(qty, selected.Quantity);

            await _storeStockService.RemoveBookQuantity(store.StoreId, selected.ISBN, qty);

            Console.WriteLine($"\n{qty} {selected.BookTitle} removed.");
            Console.ReadLine();
        }
    }
}

