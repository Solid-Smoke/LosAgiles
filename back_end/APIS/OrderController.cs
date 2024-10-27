using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        public OrderController() { }

        [HttpGet("GetPendingOrders")]
        public ActionResult<List<OrderModel>> GetPendingOrders(
        [FromServices] GetPendingOrders GetPendingOrders) {
            var pendingOrders = GetPendingOrders.Execute();
            return Ok(pendingOrders);
        }

        [HttpGet("GetProductsByOrderID/{id}")]
        public ActionResult<List<OrderProductsModel>> GetProductsByOrderID( string id,
        [FromServices] GetProductsByOrderID GetProductsByOrderID) {
            var productsInOrder = GetProductsByOrderID.Execute(id);
            return Ok(productsInOrder);
        }

        //ApproveOrder

        //RejectOrder
    }
}
