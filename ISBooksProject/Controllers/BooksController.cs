using ISDomain.DomainModels;
using ISDomain.DTO;
using ISServices.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ISHomework.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IUserService _userService;

        public BooksController(IBookService bookService, IUserService userService)
        {
            _bookService = bookService;
            _userService = userService;
        }

        // GET: BooksController
        public ActionResult Index()
        {
            var books = new BookDto
            {
                Books = _bookService.GetAllBooks(),
                Date = DateTime.Now
            };
            return View(books);
        }
        [HttpPost]
        public IActionResult Index(BookDto dto)
        {

            var books =  _bookService.GetAllBooks()
                    .Where(z => z.Date == dto.Date).ToList();
            //return View(model);

            if (!String.IsNullOrEmpty(dto.searchString))
            {
                books = _bookService.GetAllBooks()
                    .Where(z => z.BookName == dto.searchString).ToList();
            }

            if (!String.IsNullOrEmpty(dto.searchAuthor))
            {
                books = _bookService.GetAllBooks()
                    .Where(z => z.BookAuthor == dto.searchAuthor).ToList();
            }

            var model = new BookDto
            {
                Books = books,
                Date = dto.Date,
                searchString = dto.searchString,
                searchAuthor = dto.searchAuthor
            };
            return View(model);
        }


        // GET: BooksController/Details/1
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: BooksController/Create
        public ActionResult Create()
        {
            string userId;

            try {
                 userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            } catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

            if(userId != null)
            {
                if (!_userService.IsManager(userId) && !_userService.IsLibrarian(userId))
                {
                    return RedirectToAction("ChangeUserRole", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: BooksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("BookName,BookGenre,BookAuthor,Synopsis,CoverImage,BookPrice,Date")] Book book)
        {
            if (ModelState.IsValid)
            {
                this._bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Edit/1
        public ActionResult Edit(Guid? t)
        {
            string userId;

            try
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

            if (userId != null)
            {
                if (!_userService.IsManager(userId) && !_userService.IsLibrarian(userId))
                {
                    return RedirectToAction("ChangeUserRole", "Account");
                }
            } else
            {
                return RedirectToAction("Login", "Account");
            }

            if (t == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(t);

            if (book == null)
            {
                return NotFound();
            }


            return View(book);
        }

        // POST: BooksController/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Id,BookName,BookGenre,BookAuthor,Synopsis,CoverImage,BookPrice,Date")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._bookService.UpdateExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: BooksController/Delete/1
        public ActionResult Delete(Guid? id)
        {
            string userId;

            try
            {
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            catch (Exception e)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(id);

            if (book == null)
            {
                return NotFound();
            }

            if (userId != null)
            {
                if (!_userService.IsManager(userId) && !_userService.IsLibrarian(userId))
                {
                    return RedirectToAction("ChangeUserRole", "Account");
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

            return View(book);
        }

        // POST: BooksController/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            this._bookService.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult AddBookToCart(Guid? id)
        {
            var model = this._bookService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookToCart([Bind("BookId", "Quantity")] AddToShoppingCartDto item)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._bookService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Books");
            }

            return View(item);
        }

        private bool BookExists(Guid id)
        {
            return this._bookService.GetDetailsForBook(id) != null;
        }
    }
}
