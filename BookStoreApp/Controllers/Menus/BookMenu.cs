using BookStoreApp.Controllers.Helpers;
using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Menus
{
    public class BookMenu
    {
        private readonly BookHelper _bookHelper;
        private readonly BookService _bookService;

        public BookMenu(BookService bookService, AuthorService authorService, DbService dbService)
        {
            _bookService = bookService;
            _bookHelper = new BookHelper(authorService, dbService, bookService);
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
                        await ListBooks();
                        break;

                    case "2":
                        await CreateBook();
                        break;

                    case "3":
                        await UpdateBook();
                        break;

                    case "4":
                        await DeleteBook();
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


        public async Task ListBooks()
        {
            Console.Clear();
            Console.WriteLine("=== BOOK LIST ===\n");

            var books = await _bookService.GetAllBooks();

            if (books == null || books.Count == 0)
            {
                Console.WriteLine("No books found.");
                Console.WriteLine("\nPress Enter to return...");
                Console.ReadLine();
                return;
            }
            var sortedBooks = books.OrderBy(b => b.Title).ToList();

            foreach (var b in sortedBooks)
            {
                Console.WriteLine($"• {b.Title}");
            }

            Console.WriteLine("\nPress Enter to return...");
            Console.ReadLine();
        }


        public async Task CreateBook()
        {
            Console.Clear();
            Console.WriteLine("=== CREATE NEW BOOK ===\n");

            var input = await _bookHelper.CollectBookInput();

            if (input == null)
            {
                Console.WriteLine("\nCreation cancelled. Returning...");
                Console.ReadLine();
                return;
            }

            var data = input.Value;

            var newBook = new Book
            {
                Isbn13 = data.isbn,
                Title = data.title,
                Price = data.price,
                ReleaseDate = data.releaseDate,
                AuthorId = data.authorId,
                GenreId = data.genreId,
                FormatId = data.formatId,
                PublisherId = data.publisherId,
                LanguagesId = data.languageId
            };

            bool success = await _bookService.CreateBook(newBook);

            Console.WriteLine(success ? "\nBook was successfully created!" : "\nFailed to create book.");
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
        }

        public async Task UpdateBook()
        {
            var books = await _bookService.GetAllBooks();
            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("Select a book to update:\n");

            int? index = InputHelper.SelectFromList(books, b => b.Title, b => books.IndexOf(b));

            if (index == null)
            {
                Console.WriteLine("\nUpdate cancelled.");
                Console.ReadLine();
                return;
            }

            var selectedBook = books[index.Value];

            bool changed = await _bookHelper.EditBookFields(selectedBook);

            if (!changed)
            {
                Console.WriteLine("\nNo changes made. Update cancelled.");
                Console.ReadLine();
                return;
            }

            bool success = await _bookService.UpdateBook(selectedBook);

            Console.WriteLine(success
                ? "\nBook updated!"
                : "\nBook update failed.");

            Console.ReadLine();
        }


        public async Task DeleteBook()
        {
            Console.Clear();
            Console.WriteLine("=== DELETE BOOK ===\n");

            var books = await _bookService.GetAllBooks();

            if (books.Count == 0)
            {
                Console.WriteLine("No books found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Select a book to delete:\n");

            int? index = InputHelper.SelectFromList(
                books,
                b => b.Title,
                b => books.IndexOf(b)
            );

            if (index == null)
            {
                Console.WriteLine("\nDeletion cancelled.");
                Console.ReadLine();
                return;
            }

            var selectedBook = books[index.Value];

            Console.Write($"\nAre you sure you want to delete \"{selectedBook.Title}\"? (y/n): ");
            string confirm = Console.ReadLine()!.ToLower();

            if (confirm != "y")
            {
                Console.WriteLine("\nDeletion cancelled.");
                Console.ReadLine();
                return;
            }

            bool success = await _bookService.DeleteBook(selectedBook.Isbn13);

            Console.WriteLine(success
                ? "\nBook deleted successfully!"
                : "\nFailed to delete book.");

            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
        }


    }
}
