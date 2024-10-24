using back_end.Domain;
using back_end.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartHandler cartHandler;

        public ShoppingCartController()
        {
            cartHandler = new ShoppingCartHandler();
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public List<ShoppingCartItemModel> Get(string id)
        {
            var businesses = cartHandler.getCart(id);
            return businesses;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            cartHandler.deleteCart(id);
        }
    }
}
