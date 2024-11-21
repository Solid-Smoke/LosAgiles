using Microsoft.AspNetCore.Mvc;
using back_end.Domain;
using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Application.Interfaces;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISubmitOrder orderCommand;

        public OrderController(ISubmitOrder orderCommand)
        {
            this.orderCommand = orderCommand;
        }

        [HttpGet("PendingOrders")]
        public ActionResult<List<OrderModel>> GetPendingOrders(
        [FromServices] GetPendingOrders GetPendingOrders)
        {
            var pendingOrders = GetPendingOrders.Execute();
            return Ok(pendingOrders);
        }

        [HttpGet("ApprovedOrders")]
        public ActionResult<List<OrderModel>> GetApprovedOrders(
        [FromServices] GetApprovedOrders GetApprovedOrders) {
            var approvedOrders = GetApprovedOrders.Execute();
            return Ok(approvedOrders);
        }

        [HttpGet("OrdersByClientID/{id}")]
        public ActionResult<List<OrderModel>> GetOrdersByClientID(string id,
        [FromServices] GetOrdersByClientID GetOrdersByClientID)
        {
            var ClientOrders = GetOrdersByClientID.Execute(id);
            return Ok(ClientOrders);
        }

        [HttpGet("ProductsByOrderID/{id}")]
        public ActionResult<List<OrderProductsModel>> GetProductsByOrderID(string id,
        [FromServices] GetProductsByOrderID GetProductsByOrderID)
        {
            var productsInOrder = GetProductsByOrderID.Execute(id);
            return Ok(productsInOrder);
        }

        [HttpPut("{id}/Approval")]
        public IActionResult ApproveOrder(string id,
        [FromServices] ApproveOrder ApproveOrder)
        {
            var wasApproved = ApproveOrder.Execute(id);
            if (wasApproved)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut("{id}/Rejection")]
        public IActionResult RejectOrder(string id,
        [FromServices] RejectOrder RejectOrder)
        {
            var wasRejected = RejectOrder.Execute(id);
            if (wasRejected)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateOrder(CreateOrderModel orderData)
        {
            return orderCommand.CreateOrder(orderData);
        }

        [HttpGet("Orders/Excluding/Completed/{userID}")]
        public ActionResult<List<OrderModel>> GetOrdersExcludingCompleted(int userID, [FromServices] IOrderHandler orderHandler)
        {
            var orders = orderHandler.GetOrdersExcludingCompleted(userID);
            return Ok(orders);
        }

        [HttpGet("Last/Ten/Purchased/{userID}")]
        public ActionResult<List<OrderProductsModel>> GetLastTenPurchased(int userID, [FromServices] GetLastTenPurchased getLastTenPurchased)
        {
            var products = getLastTenPurchased.Execute(userID);
            return Ok(products);
        }

    }
}
