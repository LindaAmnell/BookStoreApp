using BookStoreApp.Controllers;
using BookStoreApp.Controllers.Menus;
using BookStoreApp.Models;
using BookStoreApp.Service;

namespace BookStoreApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var context = new BookStoreContext();

            var dbService = new DbService(context);

            var authorService = new AuthorService(context, dbService);
            var bookService = new BookService(context, dbService);
            var storeStockService = new StoreStockService(context, dbService);

            var bookMenu = new BookMenu(bookService, authorService, dbService);
            var authorMenu = new AuthorMenu();
            var storeMenu = new StoreMenu(storeStockService, dbService, bookService);


            var app = new AppController(bookMenu, authorMenu, storeMenu);


            await app.Run();
        }
    }
}
