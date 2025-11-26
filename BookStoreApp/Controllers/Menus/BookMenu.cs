using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class BookMenu
    {
        private readonly BookHelper _bookHelper;

        public BookMenu(BookService bookService, AuthorService authorService, DbService dbService)
        {
            _bookHelper = new BookHelper(bookService, authorService, dbService);
        }

        public async Task BooksMenu()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== BOOKS MENU ===\n");
                Console.WriteLine("1. List all books");
                Console.WriteLine("2. Create new book");
                Console.WriteLine("3. Update book");
                Console.WriteLine("4. Delete book");
                Console.WriteLine("0. Back\n");

                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await _bookHelper.ListBooks();
                        break;

                    case "2":
                        await _bookHelper.CreateBook();
                        break;

                    case "3":
                        await _bookHelper.UpdateBook();
                        break;

                    case "4":
                        await _bookHelper.DeleteBook();
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
