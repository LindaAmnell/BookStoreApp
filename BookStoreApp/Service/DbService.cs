using BookStoreApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Service
{
    public class DbService
    {
        private readonly BookStoreContext _context;

        public DbService(BookStoreContext context)
        {
            _context = context;
        }


        public async Task Create<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAll<T>() where T : class
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByID<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

    }
}
