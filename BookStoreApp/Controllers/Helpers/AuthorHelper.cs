using BookStoreApp.Service;

namespace BookStoreApp.Controllers.Helpers
{

    public class AuthorHelper
    {
        private readonly AuthorService _authorService;
        private readonly BookService _bookService;
        private readonly DbService _dbService;

        public AuthorHelper(AuthorService authorService, BookService bookService, DbService dbService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _dbService = dbService;
        }





    }
}
