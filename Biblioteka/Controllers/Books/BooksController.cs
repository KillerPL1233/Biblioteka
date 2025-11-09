using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.ViewControllers.Books
{
    public class BooksController : Controller
    {
        private readonly LibraryDbContext _db;

        public BooksController(LibraryDbContext db)
        {
            _db = db;
        }

        // Lista książek
        public async Task<IActionResult> Index()
        {
            var books = await _db.Books
                .Include(b => b.Category)
                .ToListAsync();

            return View(books);
        }

        // Szczegóły książki
        public async Task<IActionResult> Details(int id)
        {
            var book = await _db.Books
                .Include(b => b.Category)
                .Include(b => b.BookTags)
                    .ThenInclude(bt => bt.Tag)
                .Include(b => b.Files)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }
    }
}
