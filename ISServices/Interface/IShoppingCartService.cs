using ISDomain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISServices.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
