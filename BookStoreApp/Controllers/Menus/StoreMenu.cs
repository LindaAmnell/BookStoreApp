using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class StoreMenu
    {
        private readonly StoreHelper _storeHelper;

        public StoreMenu(StoreStockService storeStockService, DbService dbService, BookService bookService)
        {
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
                        await _storeHelper.ListStoreStock();
                        break;
                    case "2":
                        //await _storeHelper.AddBookToStore();
                        break;
                    case "3":
                        //await _storeHelper.RemoveBookFromStore();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}
