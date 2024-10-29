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
        public ActionResult<List<ShoppingCartItemDataModel>> getUserCart(
            string id,
            [FromServices] GetUserShoppingCart getUserShoppingCartQuery)
        {
            var cartItems = getUserShoppingCartQuery.Execute(id);
            return Ok(cartItems);
        }

        [HttpPost("{id}")]
        public IActionResult AddItemToCart(
            string id,
            [FromBody] ShoppingCartItemModel item,
            [FromServices] AddItemToShoppingCart addItemToShoppingCartCommand)
        {
            var wasAdded = addItemToShoppingCartCommand.Execute(id, item);

            if (wasAdded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(
            string id,
            [FromServices] DeleteUserShoppingCart deleteUserShoppingCartCommand)
        {
            var wasDeleted = deleteUserShoppingCartCommand.Execute(id);

            if (wasDeleted)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("{id}/Verify")]
        public ActionResult<List<ShoppingCartItemDataModel>> validateUserCartQuantities(
            string id,
            [FromServices] GetShoppingCartInvalidItems userCartInvalidCart)
        {
            var invalidProducts = userCartInvalidCart.Execute(id);

            return Ok(invalidProducts);
        }

        [HttpDelete("{id}/DeleteInvalidProducts")]
        public IActionResult deleteCartItems(
            string id,
            [FromBody] List<ShoppingCartItemDataModel> itemsToDelete,
            [FromServices] DeleteInvalidProductsFromUserCart deleteItemsFromUserCartCommand)
        {
            deleteItemsFromUserCartCommand.Execute(id, itemsToDelete);
            return Ok();
        }
    }
}
