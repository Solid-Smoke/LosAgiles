using Microsoft.AspNetCore.Mvc;
using back_end.Domain;
using back_end.Application.Commands;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderCommand orderCommand;

        public OrdersController(IOrderCommand orderCommand)
        {
            this.orderCommand = orderCommand;
        }   

        [HttpPost]
        public async Task<ActionResult<bool>> createOrder(OrderModel orderData)
        {
            return orderCommand.createOrder(orderData);
        }
    }
}
