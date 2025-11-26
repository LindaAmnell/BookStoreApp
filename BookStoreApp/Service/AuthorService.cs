using BookStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Service
{
    public class AuthorService
    {

        private readonly BookStoreContext _context;
        private readonly DbService _dbService;


        public AuthorService(BookStoreContext context, DbService dbService)
        {
            _context = context;
            _dbService = dbService;
        }

        public async Task<List<Author>> GetAllAuthor()
        {
            return await _dbService.GetAll<Author>();
        }
        public async Task<bool> CreateAuthor(Author author)
        {
            try
            {
                await _dbService.Create(author);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAuthor(Author author)
        {
            var updateAuthor = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorId == author.AuthorId);


            if (updateAuthor == null)
            {
                return false;
            }

            updateAuthor.FirstName = author.FirstName;
            updateAuthor.LastName = author.LastName;
            updateAuthor.Birthdate = author.Birthdate;


            try
            {
                await _dbService.Update(updateAuthor);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var findAuthor = await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.AuthorId == id);
            if (findAuthor == null)
            {
                return false;
            }

            if (findAuthor.Books != null && findAuthor.Books.Count > 0)
            {
                return false;
            }


            try
            {
                await _dbService.Delete(findAuthor);
                return true;
            }
            catch
            {
                return false;
            }

        }



    }
}
