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
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        // GET: AuthorController
        public ActionResult Index()
        {
            var authors = new AuthorDto
            {
                Authors = _authorService.GetAllAuthors(),
                Date = DateTime.Now
            };
            return View(authors);
        }
        [HttpPost]
        public IActionResult Index(AuthorDto dto)
        {
            var authors = _authorService.GetAllAuthors()
                .Where(z => z.Birthday == dto.Date).ToList();
            var model = new AuthorDto
            {
                Authors = authors,
                Date = dto.Date
            };
            return View(model);
        }

        // GET: AuthorController/Details/1
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = this._authorService.GetDetailsForAuthor(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,Birthday,Biography,Country,Image")] Author author)
        {
            if (ModelState.IsValid)
            {
                this._authorService.CreateNewAuthor(author);
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: AuthorController/Edit/1
        public ActionResult Edit(Guid? a)
        {
            if (a == null)
            {
                return NotFound();
            }

            var author = this._authorService.GetDetailsForAuthor(a);

            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: AuthorController/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, [Bind("Name,Birthday,Biography,Country,Image")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._authorService.UpdateExistingAuthor(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: AuthorController/Delete/1
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = this._authorService.GetDetailsForAuthor(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: AuthorController/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            this._authorService.DeleteAuthor(id);
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(Guid id)
        {
            return this._authorService.GetDetailsForAuthor(id) != null;
        }
    }
}
