using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class AuthorMenu
    {
        private readonly AuthorHelper _authorHelper;

        public AuthorMenu(AuthorService authorService, BookService bookService, DbService dbService)
        {
            _authorHelper = new AuthorHelper(authorService, bookService, dbService);
        }


        public async Task AuthorMenuOptions()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== AUTHOR MENU ===\n");
                Console.WriteLine("1. List authors");
                Console.WriteLine("2. Create author");
                Console.WriteLine("3. Update author");
                Console.WriteLine("4. Delete author");
                Console.WriteLine("0. Back\n");

                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        //await _authorHelper.ListAuthors();
                        break;

                    case "2":
                        //await _authorHelper.CreateAuthor();
                        break;

                    case "3":
                        //await _authorHelper.UpdateAuthor();
                        break;

                    case "4":
                        //await _authorHelper.DeleteAuthor();
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
    }

}
