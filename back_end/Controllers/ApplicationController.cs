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
        public List<Users>getAllUsersData(int offset, int maxRows)
        {
            return handler.getAllUsersData(offset, maxRows);
        }

        [HttpGet("[action]")]
        public int getUserCount()
        {
            return handler.getUserCount();
        }
    }
}
