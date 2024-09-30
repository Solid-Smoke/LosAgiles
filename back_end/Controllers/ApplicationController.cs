using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using back_end.Handlers;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ApplicationHandler handler;

        public ShopController()
        {
            handler = new ApplicationHandler();
        }

        [HttpGet("[action]/details")]
        public async Task<ActionResult<List<ClientsAddresses>>> getAllClientAddresses(int userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest();
                }
                return new JsonResult(handler.getAllClientAddresses(userId));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> storeClientAddress(ClientsAddress address)
        {
            try
            {
                if (address == null)
                {
                    return BadRequest();
                }
                ApplicationHandler handler = new ApplicationHandler();
                var result = handler.storeClientAddress(address);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error storing address");
            }
        }
    }
}
