using ISDomain.DomainModels;
using ISDomain.DTO;
using ISDomain.Identity;
using ISServices.Interface;
using ClosedXML.Excel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ISHomework.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IBookService _bookService;
        private readonly UserManager<LibraryUser> _userManager;
        public AdminController(IUserService userService, IBookService bookService, UserManager<LibraryUser> userManager)
        {
            _userManager = userManager;
            _bookService = bookService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_userService.IsManager(userId))
            {
                return RedirectToAction("ChangeUserRole", "Account");
            }
            return View();
        }
        [HttpPost]
        public IActionResult ImportUsers(IFormFile file)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_userService.IsManager(userId) && !_userService.IsLibrarian(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<UserRegistrationDto> users = getAllUsersFromFile(file.FileName);

            bool status = true;
            foreach (var item in users)
            {
                var userCheck = _userManager.FindByEmailAsync(item.Email).Result;
                Role role;
                if (item.UserRole == 0)
                    role = Role.ROLE_MANAGER;
                else role = Role.ROLE_GUEST;

                if (userCheck == null)
                {
                    var user = new LibraryUser
                    {
                        UserName = item.Email,
                        NormalizedUserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,
                        ShoppingCart = new ShoppingCart(),
                        Role = role
                    };
                    var result = _userManager.CreateAsync(user, item.Password).Result;
                    status = status && result.Succeeded;
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("Index", "Admin");
        }

        private List<UserRegistrationDto> getAllUsersFromFile(string fileName)
        {

            List<UserRegistrationDto> users = new List<UserRegistrationDto>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        Role userRole;
                        if (reader.GetValue(3).Equals("ROLE_ADMIN"))
                        {
                            userRole = Role.ROLE_MANAGER;
                        }
                        else userRole = Role.ROLE_GUEST;
                        users.Add(new UserRegistrationDto
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString(),
                            UserRole = userRole
                        });
                    }
                }
            }
            return users;
        }

        [HttpPost]
        public IActionResult ImportBooks(IFormFile file)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!_userService.IsManager(userId) && !_userService.IsLibrarian(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\Files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
            }

            List<Book> books = getAllBooksFromFile(file.FileName);

            foreach (var item in books)
            {
                var bookCheck = _bookService.GetDetailsForBook(item.Id);
                Genre genre;
                if ((int)item.BookGenre == 0)
                    genre = Genre.Poem;
                else if ((int)item.BookGenre == 1)
                    genre = Genre.Stories;
                else if ((int)item.BookGenre == 2)
                    genre = Genre.Fantasy;
                else if ((int)item.BookGenre == 3)
                    genre = Genre.Science;
                else if ((int)item.BookGenre == 4)
                    genre = Genre.Fable;
                else genre = Genre.Philosophy;
                if (bookCheck == null)
                {
                    var book = new Book
                    {
                        BookName = item.BookName,
                        BookGenre = genre,
                        BookAuthor = item.BookAuthor,
                        Synopsis = item.Synopsis,
                        CoverImage = item.CoverImage,
                        BookPrice = item.BookPrice,
                        Date = DateTime.Now
                    };
                     _bookService.CreateNewBook(book);
                }
                else
                {
                    continue;
                }
            }

            return RedirectToAction("Index", "Admin");
        }

        private List<Book> getAllBooksFromFile(string fileName)
        {

            List<Book> books = new List<Book>();
            string filePath = $"{Directory.GetCurrentDirectory()}\\Files\\{fileName}";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        Genre genre;
                        if (reader.GetValue(1).Equals("Philosophy"))
                        {
                            genre = Genre.Philosophy;
                        }
                        else if (reader.GetValue(1).Equals("Poem"))
                            genre = Genre.Poem;
                        else if (reader.GetValue(1).Equals("Stories"))
                            genre = Genre.Stories;
                        else if (reader.GetValue(1).Equals("Fantasy"))
                            genre = Genre.Fantasy;
                        else if (reader.GetValue(1).Equals("Science"))
                            genre = Genre.Science;
                        else if (reader.GetValue(1).Equals("Fable"))
                            genre = Genre.Fable;
                        else genre = Genre.Philosophy;
                        books.Add(new Book
                        {
                            BookName = reader.GetValue(0).ToString(),
                            BookGenre = genre,
                            BookAuthor = reader.GetValue(2).ToString(),
                            Synopsis = reader.GetValue(3).ToString(),
                            CoverImage = reader.GetValue(4).ToString(),
                            BookPrice = (int)((double)reader.GetValue(5)),
                            Date = DateTime.Now
                        });
                    }
                }
            }
            return books;
        }
        [HttpPost]
        public FileContentResult ExportBooksFromGenre([Bind("BookGenre")] Book book)
        {
            var genre = book.BookGenre;

            string fileName = "books"+genre+".xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(genre.ToString());

                worksheet.Cell(1, 1).Value = "Book Id";
                worksheet.Cell(1, 2).Value = "Book Name";
                worksheet.Cell(1, 3).Value = "Book Author";
                worksheet.Cell(1, 4).Value = "Book Genre";
                worksheet.Cell(1, 5).Value = "Book Price";
                worksheet.Cell(1, 6).Value = "Date of Publishing";

                var result = this._bookService.GetAllBooks().Where(z => z.BookGenre == genre).ToList();

                if (result.Count > 0)
                {
                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.BookName.ToString();
                        worksheet.Cell(i + 1, 3).Value = item.BookAuthor.ToString();
                        worksheet.Cell(i + 1, 4).Value = item.BookGenre.ToString();
                        worksheet.Cell(i + 1, 5).Value = item.BookPrice.ToString();
                        worksheet.Cell(i + 1, 6).Value = item.Date.ToString();
                    }
                }
                else
                {
                    worksheet.Cell(2, 1).Value = "No books for selected genre";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult ExportAllBooks()
        {

            string fileName = "AllBooks.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("AllBooks");

                worksheet.Cell(1, 1).Value = "Book Id";
                worksheet.Cell(1, 2).Value = "Book Name";
                worksheet.Cell(1, 3).Value = "Book Author";
                worksheet.Cell(1, 4).Value = "Book Genre";
                worksheet.Cell(1, 5).Value = "Book Price";
                worksheet.Cell(1, 6).Value = "Date of Publishing";

                var result = _bookService.GetAllBooks().ToList();

                if (result.Count > 0)
                {
                    for (int i = 1; i <= result.Count(); i++)
                    {
                        var item = result[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.BookName.ToString();
                        worksheet.Cell(i + 1, 3).Value = item.BookAuthor.ToString();
                        worksheet.Cell(i + 1, 4).Value = item.BookGenre.ToString();
                        worksheet.Cell(i + 1, 5).Value = item.BookPrice.ToString();
                        worksheet.Cell(i + 1, 6).Value = item.Date.ToString();

                    }
                }
                else
                {
                    worksheet.Cell(2, 1).Value = "No books published on this date";
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }
    }
}
