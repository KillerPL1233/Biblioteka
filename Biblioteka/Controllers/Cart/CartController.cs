using Biblioteka.Data;
using Biblioteka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteka.ViewControllers.Cart
{
    public class CartController : Controller
    {
        private readonly LibraryDbContext _db;

        public CartController(LibraryDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // Na razie pobieramy wszystkie książki jako przykład
            var cartItems = await _db.Books
                .Include(b => b.Category)
                .ToListAsync();

            return View(cartItems);
        }

        // TODO w przyszłości: dodawanie / usuwanie książek
    }
}
