using BookStoreApp.Controllers.Menus;

namespace BookStoreApp.Controllers
{

    public class AppController
    {
        private readonly BookMenu _bookController;
        private readonly AuthorMenu _authorController;
        private readonly StoreMenu _storeController;

        public AppController(
            BookMenu bookMenu,
            AuthorMenu authorMenu,
            StoreMenu storeMenu)
        {
            _bookController = bookMenu;
            _authorController = authorMenu;
            _storeController = storeMenu;
        }

        public async Task Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== BOOKSTORE APP ===\n");
                Console.WriteLine("1. Books");
                Console.WriteLine("2. Authors");
                Console.WriteLine("3. Store Stock");
                Console.WriteLine("0. Exit\n");

                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await _bookController.BooksMenu();
                        break;

                    case "2":
                        await _authorController.AuthorsMenu();
                        break;

                    case "3":
                        await _storeController.StoreMenuScreen();
                        break;

                    case "0":
                        Console.WriteLine("Exiting...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Press Enter to continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }
    }

}










//public async Task DeleteAuthorMenu()
//{
//    Console.Clear();
//    Console.WriteLine("=== DELETE AUTHOR ===\n");

//    // 1. Hämta alla författare
//    var authors = await _authorService.GetAllAuthor();

//    if (authors.Count == 0)
//    {
//        Console.WriteLine("No authors found.");
//        return;
//    }

//    // 2. Visa författare i numrerad lista
//    for (int i = 0; i < authors.Count; i++)
//    {
//        Console.WriteLine($"{i + 1}. {authors[i].FirstName} {authors[i].LastName}");
//    }

//    // 3. Välj författare
//    Console.Write("\nSelect author to delete: ");
//    if (!int.TryParse(Console.ReadLine(), out int choice))
//    {
//        Console.WriteLine("Invalid choice.");
//        return;
//    }

//    if (choice < 1 || choice > authors.Count)
//    {
//        Console.WriteLine("Invalid author number.");
//        return;
//    }

//    var selectedAuthor = authors[choice - 1];

//    // 4. Bekräftelse
//    Console.WriteLine($"\nAre you sure you want to delete {selectedAuthor.FirstName} {selectedAuthor.LastName}? (y/n)");
//    var confirm = Console.ReadLine()?.ToLower();

//    if (confirm != "y")
//    {
//        Console.WriteLine("Cancelled.");
//        return;
//    }

//    // 5. Försök ta bort
//    bool result = await _authorService.DeleteAuthor(selectedAuthor.AuthorId);

//    if (result)
//    {
//        Console.WriteLine("Author deleted successfully!");
//    }
//    else
//    {
//        Console.WriteLine("Could not delete author. They may have books linked to them.");
//    }
//}