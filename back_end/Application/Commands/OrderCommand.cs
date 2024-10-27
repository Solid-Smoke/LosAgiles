using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public interface IOrderCommand
    {
        bool createOrder(OrderModel orderData);
    }

    public class OrderCommand : IOrderCommand
    {
        private readonly IOrdersHandler _ordersHandler;

        public OrderCommand(IOrdersHandler ordersHandler)
        {
            _ordersHandler = ordersHandler;
        }

        public bool createOrder(OrderModel orderData)
        {
            try
            {
                return _ordersHandler.createOrder(orderData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
