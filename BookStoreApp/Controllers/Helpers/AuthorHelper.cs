using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Helpers
{

    public class AuthorHelper
    {
        private readonly AuthorService _authorService;

        public AuthorHelper(AuthorService authorService)
        {
            _authorService = authorService;
        }

        public (string firstName, string lastName, DateOnly birthdate)? CollectAuthorInput()
        {
            // FIRST NAME
            Console.Write("Enter first name (or 0 to cancel): ");
            string? firstName = Console.ReadLine();

            while (true)
            {
                if (firstName == "0")
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(firstName))
                    break;

                Console.Write("First name cannot be empty. Enter first name: ");
                firstName = Console.ReadLine();
            }

            // LAST NAME
            Console.Write("Enter last name (or 0 to cancel): ");
            string? lastName = Console.ReadLine();

            while (true)
            {
                if (lastName == "0")
                {
                    return null;
                }
                if (!string.IsNullOrWhiteSpace(lastName))
                    break;
                Console.Write("Last name cannot be empty. Enter last name: ");
                lastName = Console.ReadLine();
            }

            // BIRTHDATE
            Console.Write("Enter birthdate YYYY-MM-DD (or 0 to cancel): ");
            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                return null;
            }
            DateOnly birthdate;
            while (!DateOnly.TryParse(dateInput, out birthdate))
            {
                Console.Write("Invalid date. Use YYYY-MM-DD (or 0 to cancel): ");
                dateInput = Console.ReadLine();

                if (dateInput == "0")
                {
                    return null;
                }
            }

            return (firstName, lastName, birthdate);

        }

        public void EditAuthorFields(Author author)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== UPDATE AUTHOR ===\n");
                Console.WriteLine($"1. First name: {author.FirstName}");
                Console.WriteLine($"2. Last name:  {author.LastName}");
                Console.WriteLine($"3. Birthdate:  {author.Birthdate}");
                Console.WriteLine("0. Finish\n");

                Console.Write("Select field to update: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("New first name: ");
                        var newFirstName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newFirstName))
                        {
                            author.FirstName = newFirstName;
                        }
                        break;

                    case "2":
                        Console.Write("New last name: ");
                        var newLastName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newLastName))
                        {
                            author.LastName = newLastName;
                        }
                        break;

                    case "3":
                        Console.Write("New birthdate (YYYY-MM-DD): ");
                        var newDateInput = Console.ReadLine();
                        if (DateOnly.TryParse(newDateInput, out var newBirthdate))
                        {
                            author.Birthdate = newBirthdate;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date. Press Enter to continue...");
                            Console.ReadLine();
                        }
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Press Enter to continue...");
                        Console.ReadLine();
                        break;
                }
            }
        }

        public async Task<Author?> SelectAuthor(string title)
        {
            Console.Clear();
            Console.WriteLine(title + "\n");

            var authors = await _authorService.GetAllAuthor();

            if (authors == null || authors.Count == 0)
            {
                Console.WriteLine("No authors found.");
                Console.ReadLine();
                return null;
            }

            int? index = InputHelper.SelectFromList(
                authors,
                a => $"{a.FirstName} {a.LastName}",
                a => authors.IndexOf(a)
            );

            if (index == null)
            {
                Console.WriteLine("\nOperation cancelled.");
                Console.ReadLine();
                return null;
            }

            return authors[index.Value];
        }

        public async Task<bool> CheckIfAuthorHasBooks(Author author, BookService bookService)
        {
            if (author.Books == null || !author.Books.Any())
            {
                return true;
            }

            Console.WriteLine($"\n{author.FirstName} {author.LastName} has the following books:");
            foreach (var book in author.Books)
            {
                Console.WriteLine($" - {book.Title}");
            }

            Console.Write("\nDo you want to delete all these books as well? (y/n): ");
            string choice = Console.ReadLine()!.ToLower();

            if (choice != "y")
            {
                return false;
            }

            Console.WriteLine("\nDeleting books...");

            foreach (var book in author.Books.ToList())
            {
                bool deleted = await bookService.DeleteBook(book.Isbn13);

                if (deleted)
                {
                    Console.WriteLine($"Deleted: {book.Title}");
                }
                else
                {
                    Console.WriteLine($"Failed to delete: {book.Title}");
                }
            }

            return true;
        }




    }
}
