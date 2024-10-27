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
        public ActionResult<List<OrderProductsModel>> GetProductsByOrderID(string id,
        [FromServices] GetProductsByOrderID GetProductsByOrderID) {
            var productsInOrder = GetProductsByOrderID.Execute(id);
            return Ok(productsInOrder);
        }

        [HttpPut("ApproveOrder/{id}")]
        public IActionResult ApproveOrder(string id,
        [FromServices] ApproveOrder ApproveOrder) {
            var wasApproved = ApproveOrder.Execute(id);
            if (wasApproved) {
                return NoContent();
            } else {
                return NotFound();
            }
        }

        [HttpPut("RejectOrder/{id}")]
        public IActionResult RejectOrder(string id,
        [FromServices] RejectOrder RejectOrder) {
            var wasRejected = RejectOrder.Execute(id);
            if (wasRejected) {
                return NoContent();
            } else {
                return NotFound();
            }
        }
    }
}
