using ISDomain.DomainModels;
using ISDomain.DTO;
using ISDomain.Identity;
using ISRepository.Interface;
using ISServices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISServices.Implementation
{
    public class BookService : IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<BookInShoppingCart> _bookInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public BookService(IRepository<Book> bookRepository, IRepository<BookInShoppingCart> bookInShoppingCartRepository, IUserRepository userRepository)
        {
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _bookInShoppingCartRepository = bookInShoppingCartRepository;
        }

        public void CreateNewBook(Book t)
        {
            this._bookRepository.Insert(t);
        }

        public void DeleteBook(Guid id)
        {
            var Book = this.GetDetailsForBook(id);
            this._bookRepository.Delete(Book);
        }

        public List<Book> GetAllBooks()
        {
            return this._bookRepository.GetAll().ToList();
        }

        public Book GetDetailsForBook(Guid? id)
        { 
            return this._bookRepository.Get(id);
        }

        public void UpdateExistingBook(Book t)
        {
            this._bookRepository.Update(t);
        }

        public bool AddToShoppingCart(AddToShoppingCartDto item, string userID)
        {

            var user = this._userRepository.Get(userID);

            var userShoppingCart = user.ShoppingCart;

            if (item.BookId != null && userShoppingCart != null)
            {
                var book = this.GetDetailsForBook(item.BookId);

                if (book != null)
                {
                    BookInShoppingCart itemToAdd = new BookInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Books = book,
                        BookId = book.Id,
                        ShoppingCart = userShoppingCart,
                        ShoppingCartId = userShoppingCart.Id,
                        Quantity = item.Quantity
                    };

                    this._bookInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }
        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var book = this.GetDetailsForBook(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                Books = book,
                BookId = book.Id,
                Quantity = 1
            };

            return model;
        }
    }
}
