using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize]
public class LibraryController : Controller
{
    private readonly ILogger<LibraryController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public LibraryController(ILogger<LibraryController> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _logger = logger;
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            List<Book> books = await _context.Books
                                         .Include(x => x.Author)
                                         .Include(x => x.Category)
                                         .ToListAsync();

            return View(books);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kitaplar alınırken bir hata oluştu.");
            return View("Error");
        }
    }

    [HttpGet]
    [HttpGet]
    public async Task<IActionResult> AddBook()
    {
        var authors = await GetAuthorsAsync();

        var model = new BookViewModel
        {
            Authors = authors
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(BookViewModel model, string action)
    {
        if (action == "AddNewAuthor")
        {
            if (!string.IsNullOrWhiteSpace(model.NewAuthorName))
            {
                var newAuthor = new Author { Name = model.NewAuthorName };
                _context.Authors.Add(newAuthor);
                await _context.SaveChangesAsync();

                model.Authors = await _context.Authors
                    .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name })
                    .ToListAsync();

                model.NewAuthorName = string.Empty;
            }
            else
            {
                ModelState.AddModelError("NewAuthorName", "Yeni yazar adı boş olamaz.");
            }

            return View(model);
        }
        else if (action == "AddBook")
        {
            if (!ModelState.IsValid)
            {
                var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == model.NewCategoryName);

                if (category == null && !string.IsNullOrWhiteSpace(model.NewCategoryName))
                {
                    // Yeni kategori ekle
                    category = new Category
                    {
                        Name = model.NewCategoryName
                    };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                }

                var book = new Book
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = model.SelectedAuthorId.Value,
                    CategoryId = category?.Id 
                };

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Library");
            }
        }

        model.Authors = await GetAuthorsAsync();
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> ReturnBook(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        if (book.ApplicationUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
        {
            TempData["Message"] = "Bu kitap başka bir kullanıcı tarafından ödünç alınmış.";
            return RedirectToAction("Index");
        }

        book.ApplicationUserId = null;
        _context.Update(book);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Kitap iade edildi.";
        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> BorrowBook(int bookId)
    {
        var book = await _context.Books.FindAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        if (book.ApplicationUserId != null)
        {
            TempData["Message"] = "Bu kitap zaten ödünç alınmış.";
            return RedirectToAction("Index");
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        book.ApplicationUserId = userId;
        _context.Update(book);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Kitap ödünç alındı.";
        return RedirectToAction("Index");
    }

    private async Task<List<SelectListItem>> GetAuthorsAsync()
    {
        try
        {
            var authors = await _context.Authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToListAsync();

            return authors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Yazarlar alınırken bir hata oluştu.");
            throw;
        }
    }

    public IActionResult Edit(int id)
    {
        var book = _context.Books.Find(id);
        if (book == null)
        {
            return NotFound();
        }

        var model = new EditBookViewModel
        {
            Id = book.Id,
            Content = book.Content
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditBookViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var book = await _context.Books.FindAsync(model.Id);
        if (book == null)
        {
            return NotFound();
        }

        book.Content = model.Content;
        _context.Update(book);
        await _context.SaveChangesAsync();

        TempData["Message"] = "Kitap içeriği başarıyla güncellendi.";
        return RedirectToAction("Index");
    }
}