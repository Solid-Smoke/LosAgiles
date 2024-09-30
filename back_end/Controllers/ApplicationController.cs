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
        public async Task<ActionResult<List<ClientsAddresses>>> getAllClientAddresses(string userName)
        {
            try
            {
                if (userName == null)
                {
                    return BadRequest();
                }
                return new JsonResult(handler.getAllClientAddresses(userName));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
