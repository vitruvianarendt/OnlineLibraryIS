using ISDomain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISRepository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        Order GetOrderDetails(Guid orderId);
    }
}
