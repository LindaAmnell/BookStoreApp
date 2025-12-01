using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class AuthorMenu
    {
        private readonly AuthorHelper _authorHelper;
        private readonly AuthorService _authorService;
        private readonly BookService _bookService;


        public AuthorMenu(AuthorService authorService, BookService bookService, DbService dbService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _authorHelper = new AuthorHelper(authorService);
        }


        public async Task AuthorsMenu()
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
                        await ListAuthors();
                        break;

                    case "2":
                        await CreateAuthor();
                        break;

                    case "3":
                        await UpdateAuthor();
                        break;

                    case "4":
                        await DeleteAuthor();
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

        public async Task ListAuthors()
        {
            Console.Clear();
            Console.WriteLine("=== AUTHOR LIST ===\n");

            var author = await _authorService.GetAllAuthor();

            if (author == null)
            {
                Console.WriteLine("No author found.");
                Console.WriteLine("\nPress Enter to return...");
                Console.ReadLine();
                return;
            }

            var sortedAuthor = author.OrderBy(a => a.FirstName).ToList();

            foreach (var a in sortedAuthor)
            {
                Console.WriteLine($"{a.FirstName} {a.LastName}");
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();

        }

        public async Task CreateAuthor()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE AUTHOR ===\n");

            var input = _authorHelper.CollectAuthorInput();

            if (input == null)
            {
                Console.WriteLine("Creation cancelled.");
                Console.ReadLine();
                return;
            }

            var data = input.Value;

            var newAuthor = new Author
            {
                FirstName = data.firstName,
                LastName = data.lastName,
                Birthdate = data.birthdate
            };

            bool success = await _authorService.CreateAuthor(newAuthor);

            Console.WriteLine(success
                ? "\nAuthor created successfully!"
                : "\nFailed to create author.");

            Console.ReadLine();
        }

        public async Task UpdateAuthor()
        {
            var author = await _authorHelper.SelectAuthor("Select an author to update:");

            if (author == null)
                return;

            _authorHelper.EditAuthorFields(author);

            bool success = await _authorService.UpdateAuthor(author);

            Console.WriteLine(success
                ? "\nAuthor updated!"
                : "\nAuthor update failed.");

            Console.ReadLine();
        }

        public async Task DeleteAuthor()
        {
            var author = await _authorHelper.SelectAuthor("Select an author to delete:");

            if (author == null)
            {
                return;
            }
            bool canDeleteAuthor = await _authorHelper.CheckIfAuthorHasBooks(author, _bookService);

            if (!canDeleteAuthor)
            {
                Console.WriteLine("\nDeletion cancelled.");
                Console.ReadLine();
                return;
            }

            Console.Write($"\nAre you sure you want to delete \"{author.FirstName} {author.LastName}\"? (y/n): ");
            string confirm = Console.ReadLine()!.ToLower();

            if (confirm != "y")
            {
                Console.WriteLine("\nDeletion cancelled.");
                Console.ReadLine();
                return;
            }

            bool success = await _authorService.DeleteAuthor(author.AuthorId);

            Console.WriteLine(success
                ? "\nAuthor deleted!"
                : "\nFailed to delete author.");

            Console.ReadLine();
        }
    }

}
