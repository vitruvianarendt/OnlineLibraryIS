using ISDomain.DomainModels;
using ISDomain.DTO;
using ISRepository.Interface;
using ISServices.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISServices.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BookInOrder> _bookInOrderRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository,
                                   IRepository<BookInOrder> bookInOrderRepository,
                                   IRepository<Order> orderRepository,
                                   IUserRepository userRepository,
                                   IRepository<EmailMessage> emailMessageRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _bookInOrderRepository = bookInOrderRepository;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggedInUser = this._userRepository.Get(userId);
                var userShoppingCart = loggedInUser.ShoppingCart;
                var itemToDelete = userShoppingCart.BooksInShoppingCart.Where(z => z.BookId.Equals(id)).FirstOrDefault();

                userShoppingCart.BooksInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepository.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.ShoppingCart;

            var tickets = userShoppingCart.BooksInShoppingCart.ToList();

            var allTicketPrice = tickets.Select(z => new
            {
                TicketPrice = z.Books.BookPrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allTicketPrice)
            {
                totalPrice += item.Quanitity * item.TicketPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Books = tickets,
                TotalPrice = totalPrice
            };

            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {

                var loggedInUser = this._userRepository.Get(userId);
                var userShoppingCart = loggedInUser.ShoppingCart;

                EmailMessage mail = new EmailMessage
                {
                    MailTo = loggedInUser.Email,
                    Subject = "Successfully created order",
                    Status = false
                };

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepository.Insert(order);

                List<BookInOrder> ticketsInOrder = new List<BookInOrder>();

                var result = userShoppingCart.BooksInShoppingCart.Select(z => new BookInOrder
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Books.Id,
                    Books = z.Books,
                    OrderId = order.Id,
                    UserOrder = order,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is finished. This order conains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    totalPrice += item.Quantity * item.Books.BookPrice;
                    sb.AppendLine(i.ToString() + ". " 
                        + item.Books.BookName + " " + item.Books.Date + ", genre: " + item.Books.BookGenre +
                        " with price: " + item.Books.BookPrice + " and amount: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                ticketsInOrder.AddRange(result);

                foreach (var item in ticketsInOrder)
                {
                    this._bookInOrderRepository.Insert(item);
                }

                loggedInUser.ShoppingCart.BooksInShoppingCart.Clear();

                this._userRepository.Update(loggedInUser);
                this._emailMessageRepository.Insert(mail);

                return true;
            }
            return false;
        }
    }
}
