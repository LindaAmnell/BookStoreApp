using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Helpers
{
    public class BookHelper
    {
        private readonly AuthorService _authorService;
        private readonly DbService _dbService;

        public BookHelper(AuthorService authorService, DbService dbService)
        {
            _dbService = dbService;
            _authorService = authorService;
        }

        private (string isbn, string title, decimal price, DateOnly releaseDate)? CollectBasicBookData()
        {
            // ISBN
            Console.Write("Enter ISBN-13 (or 0 to cancel): ");
            string? isbn = Console.ReadLine();

            while (true)
            {
                if (isbn == "0")
                    return null;

                if (!string.IsNullOrWhiteSpace(isbn))
                    break;

                Console.Write("ISBN cannot be empty. Enter ISBN-13: ");
                isbn = Console.ReadLine();
            }

            // Title
            Console.Write("Enter book title (or 0 to cancel): ");
            string? title = Console.ReadLine();

            while (true)
            {
                if (title == "0")
                    return null;

                if (!string.IsNullOrWhiteSpace(title))
                    break;

                Console.Write("Title cannot be empty. Enter book title: ");
                title = Console.ReadLine();
            }

            // Price
            Console.Write("Enter price (or 0 to cancel): ");
            string? priceInput = Console.ReadLine();

            if (priceInput == "0")
                return null;

            decimal price;
            while (!decimal.TryParse(priceInput, out price))
            {
                Console.Write("Invalid price. Enter valid price: ");
                priceInput = Console.ReadLine();

                if (priceInput == "0")
                    return null;
            }
            // Release date
            Console.Write("Enter release date YYYY-MM-DD (or 0 to cancel): ");
            string? dateInput = Console.ReadLine();

            if (dateInput == "0")
                return null;

            DateOnly releaseDate;
            while (!DateOnly.TryParse(dateInput, out releaseDate))
            {
                Console.Write("Invalid date. Use YYYY-MM-DD: ");
                dateInput = Console.ReadLine();

                if (dateInput == "0")
                    return null;
            }

            return (isbn, title, price, releaseDate);
        }


        private async Task<(int authorId, int genreId, int formatId, int publisherId, int languageId)?>
           CollectBookSelections((string isbn, string title, decimal price, DateOnly releaseDate) basic)
        {
            string? authorName = null;
            string? genreName = null;
            string? formatName = null;
            string? publisherName = null;
            string? languageName = null;

            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            // AUTHOR
            Console.WriteLine("\nSelect an author:");
            var authors = await _authorService.GetAllAuthor();
            int? authorId = InputHelper.SelectFromList(authors, a => $"{a.FirstName} {a.LastName}", a => a.AuthorId);
            if (authorId == null) return null;

            authorName = authors.First(a => a.AuthorId == authorId).FirstName
                 + " " + authors.First(a => a.AuthorId == authorId).LastName;
            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            // GENRE
            Console.WriteLine("\nSelect a genre:");
            var genres = await _dbService.GetAll<Genre>();
            int? genreId = InputHelper.SelectFromList(genres, g => g.GenreName, g => g.GenreId);
            if (genreId == null) return null;

            genreName = genres.First(g => g.GenreId == genreId).GenreName;
            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            // FORMAT
            Console.WriteLine("\nSelect a format:");
            var formats = await _dbService.GetAll<Format>();
            int? formatId = InputHelper.SelectFromList(formats, f => f.FormatType, f => f.FormatId);
            if (formatId == null) return null;

            formatName = formats.First(f => f.FormatId == formatId).FormatType;
            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            // PUBLISHER
            Console.WriteLine("\nSelect a publisher:");
            var publishers = await _dbService.GetAll<Publisher>();
            int? publisherId = InputHelper.SelectFromList(publishers, p => p.Name, p => p.PublisherId);
            if (publisherId == null) return null;

            publisherName = publishers.First(p => p.PublisherId == publisherId).Name;
            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            // LANGUAGE
            Console.WriteLine("\nSelect a language:");
            var languages = await _dbService.GetAll<Language>();
            int? languageId = InputHelper.SelectFromList(languages, l => l.LanguageName, l => l.LanguageId);
            if (languageId == null) return null;

            languageName = languages.First(l => l.LanguageId == languageId).LanguageName;
            SelectedInfo(basic, authorName, genreName, formatName, publisherName, languageName);

            return (authorId.Value, genreId.Value, formatId.Value, publisherId.Value, languageId.Value);
        }

        public async Task<(string isbn, string title, decimal price, DateOnly releaseDate,
               int authorId, int genreId, int formatId, int publisherId, int languageId)?>
             CollectBookInput()
        {
            var basic = CollectBasicBookData();
            if (basic == null)
                return null;

            var selections = await CollectBookSelections(basic.Value);
            if (selections == null)
                return null;

            return (
                basic.Value.isbn,
                basic.Value.title,
                basic.Value.price,
                basic.Value.releaseDate,
                selections.Value.authorId,
                selections.Value.genreId,
                selections.Value.formatId,
                selections.Value.publisherId,
                selections.Value.languageId
            );
        }

        public async Task EditBookFields(Book book)
        {
            var authors = await _authorService.GetAllAuthor();
            var genres = await _dbService.GetAll<Genre>();
            var formats = await _dbService.GetAll<Format>();
            var publishers = await _dbService.GetAll<Publisher>();
            var languages = await _dbService.GetAll<Language>();

            bool running = true;

            while (running)
            {
                string authorName = authors.FirstOrDefault(a => a.AuthorId == book.AuthorId) is var a && a != null
                    ? $"{a.FirstName} {a.LastName}"
                    : "Unknown";

                string genreName = genres.FirstOrDefault(g => g.GenreId == book.GenreId)?.GenreName ?? "Unknown";
                string formatName = formats.FirstOrDefault(f => f.FormatId == book.FormatId)?.FormatType ?? "Unknown";
                string publisherName = publishers.FirstOrDefault(p => p.PublisherId == book.PublisherId)?.Name ?? "Unknown";
                string languageName = languages.FirstOrDefault(l => l.LanguageId == book.LanguagesId)?.LanguageName ?? "Unknown";

                Console.Clear();
                Console.WriteLine("=== UPDATE BOOK ===\n");
                Console.WriteLine($"1. Title:        {book.Title}");
                Console.WriteLine($"2. Price:        {book.Price}");
                Console.WriteLine($"3. Release date: {book.ReleaseDate}");
                Console.WriteLine($"4. Author:       {authorName}");
                Console.WriteLine($"5. Genre:        {genreName}");
                Console.WriteLine($"6. Format:       {formatName}");
                Console.WriteLine($"7. Publisher:    {publisherName}");
                Console.WriteLine($"8. Language:     {languageName}");
                Console.WriteLine("0. Finish\n");

                Console.Write("Select field to update: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("New title: ");
                        var newTitle = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newTitle))
                            book.Title = newTitle;
                        break;

                    case "2":
                        Console.Write("New price: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                            book.Price = newPrice;
                        break;

                    case "3":
                        Console.Write("New release date (YYYY-MM-DD): ");
                        if (DateOnly.TryParse(Console.ReadLine(), out var newDate))
                            book.ReleaseDate = newDate;
                        break;

                    case "4":
                        Console.WriteLine("Choose a new author:");
                        int? authorId = InputHelper.SelectFromList(authors,
                            a => $"{a.FirstName} {a.LastName}",
                            a => a.AuthorId);

                        if (authorId != null)
                            book.AuthorId = authorId.Value;
                        break;

                    case "5":
                        Console.WriteLine("Choose a new genre:");
                        int? genreId = InputHelper.SelectFromList(genres, g => g.GenreName, g => g.GenreId);

                        if (genreId != null)
                            book.GenreId = genreId.Value;
                        break;

                    case "6":
                        Console.WriteLine("Choose a new format:");
                        int? formatId = InputHelper.SelectFromList(formats, f => f.FormatType, f => f.FormatId);

                        if (formatId != null)
                            book.FormatId = formatId.Value;
                        break;

                    case "7":
                        Console.WriteLine("Choose a new publisher:");
                        int? publisherId = InputHelper.SelectFromList(publishers, p => p.Name, p => p.PublisherId);

                        if (publisherId != null)
                            book.PublisherId = publisherId.Value;
                        break;

                    case "8":
                        Console.WriteLine("Choose a new language:");
                        int? langId = InputHelper.SelectFromList(languages, l => l.LanguageName, l => l.LanguageId);

                        if (langId != null)
                            book.LanguagesId = langId.Value;
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Console.ReadLine();
                        break;
                }
            }
        }
        private void SelectedInfo(
            (string isbn, string title, decimal price, DateOnly releaseDate) basic,
                    string? author,
                    string? genre,
                    string? format,
                    string? publisher,
                    string? language)
        {
            Console.Clear();
            Console.WriteLine("==== ADD BOOK ===");

            Console.WriteLine($"ISBN: {basic.isbn}");
            Console.WriteLine($"TITLE: {basic.title}");
            Console.WriteLine($"PRICE: {basic.price}");
            Console.WriteLine($"RELEASE DATE: {basic.releaseDate.ToShortDateString()}");
            if (!string.IsNullOrWhiteSpace(author))
                Console.WriteLine($"AUTHOR: {author}");

            if (!string.IsNullOrWhiteSpace(genre))
                Console.WriteLine($"GENRE: {genre}");

            if (!string.IsNullOrWhiteSpace(format))
                Console.WriteLine($"FORMAT: {format}");

            if (!string.IsNullOrWhiteSpace(publisher))
                Console.WriteLine($"PUBLISHER: {publisher}");

            if (!string.IsNullOrWhiteSpace(language))
                Console.WriteLine($"LANGUAGE: {language}");

            Console.WriteLine();

        }

    }
}
