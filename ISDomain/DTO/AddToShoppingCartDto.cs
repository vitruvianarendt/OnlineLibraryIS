using ISDomain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISDomain.DTO
{
    public class AddToShoppingCartDto
    {
        public Book Books { get; set; }
        public Guid BookId { get; set; }
        public int Quantity { get; set; }

    }
}
