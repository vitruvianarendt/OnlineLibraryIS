using ISDomain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string UserId { get; set; }
        public LibraryUser User { get; set; }
        public virtual ICollection<BookInShoppingCart> BooksInShoppingCart { get; set; }
    }
}
