using back_end.Application.Commands;
using back_end.Application.Queries;
using back_end.Domain;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        public ShoppingCartController() { }

        [HttpGet("{id}")]
        public ActionResult<List<ShoppingCartItemModel>> getUserCart(
            string id,
            [FromServices] GetUserShoppingCart getUserShoppingCartQuery)
        {
            var cartItems = getUserShoppingCartQuery.Execute(id);
            return Ok(cartItems);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            string id,
            [FromServices] DeleteUserShoppingCart deleteUserShoppingCartCommand)
        {
            var wasDeleted = deleteUserShoppingCartCommand.Execute(id);

            if (wasDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }
    }
}
