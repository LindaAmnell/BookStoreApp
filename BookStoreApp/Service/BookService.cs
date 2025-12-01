using BookStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Service
{
    public class BookService
    {
        private readonly BookStoreContext _context;
        private readonly DbService _dbService;


        public BookService(BookStoreContext context, DbService dbService)
        {
            _context = context;
            _dbService = dbService;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();

        }


        public async Task<bool> CreateBook(Book book)
        {
            try
            {
                await _dbService.Create(book);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Book?> GetBookByISBN(string isbn)
        {
            return await _context.Books.FindAsync(isbn);
        }

        public async Task<bool> UpdateBook(Book book)
        {

            var updatebook = await _context.Books.FirstOrDefaultAsync(s => s.Isbn13 == book.Isbn13);

            if (updatebook == null)
            {
                return false;
            }

            updatebook.Title = book.Title;
            updatebook.Price = book.Price;
            updatebook.LanguagesId = book.LanguagesId;
            updatebook.ReleaseDate = book.ReleaseDate;
            updatebook.AuthorId = book.AuthorId;
            updatebook.GenreId = book.GenreId;
            updatebook.FormatId = book.FormatId;
            updatebook.PublisherId = book.PublisherId;
            try
            {
                await _dbService.Update(updatebook);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBook(string isbn)
        {
            var findBook = await _context.Books.FirstOrDefaultAsync(s => s.Isbn13 == isbn);

            if (findBook == null)
            {
                return false;
            }

            try
            {
                await _dbService.Delete(findBook);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
