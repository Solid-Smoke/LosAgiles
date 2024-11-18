using back_end.Application.Interfaces;
using back_end.Domain;

namespace back_end.Application.Commands
{
    public interface ISubmitOrder
    {
        bool CreateOrder(CreateOrderModel orderData);
    }

    public class SubmitOrder : ISubmitOrder
    {
        private readonly IOrderHandler _ordersHandler;

        public SubmitOrder(IOrderHandler ordersHandler)
        {
            _ordersHandler = ordersHandler;
        }

        public bool CreateOrder(CreateOrderModel orderData)
        {
            if (orderData.Products == null)
            {
                throw new ArgumentException("Products list cannot be null");
            }   
            try
            {
                return _ordersHandler.CreateOrder(orderData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
