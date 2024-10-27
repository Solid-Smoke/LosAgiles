using back_end.Domain;

namespace back_end.Application.Interfaces
{
    public interface IOrdersHandler
    {
        bool createOrder(OrderModel orderData);
    }
}