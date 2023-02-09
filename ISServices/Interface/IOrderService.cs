using ISDomain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISServices.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(Guid orderId);
    }
}
