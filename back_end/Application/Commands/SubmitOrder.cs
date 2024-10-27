using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public interface ISubmitOrder
    {
        bool createOrder(CreateOrderModel orderData);
    }

    public class SubmitOrder : ISubmitOrder
    {
        private readonly IOrderHandler _ordersHandler;

        public SubmitOrder(IOrderHandler ordersHandler)
        {
            _ordersHandler = ordersHandler;
        }

        public bool createOrder(CreateOrderModel orderData)
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
