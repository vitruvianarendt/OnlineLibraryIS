using ISDomain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public LibraryUser User { get; set; }
        public IEnumerable<BookInOrder> BooksInOrder { get; set; }
    }
}
