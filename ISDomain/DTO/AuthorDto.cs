using ISDomain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.DTO
{
    public class AuthorDto
    {
        public List<Author> Authors { get; set; }
        public DateTime Date { get; set; }
    }
}
