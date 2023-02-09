using ISDomain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.DTO
{
    public class BookDto
    {
        public List<Book> Books { get; set; }
        public DateTime Date { get; set; }
        public string searchString { get; set; }
        public string searchAuthor { get; set; }

    }
}
