using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsHandler clientsHandler;

        public ClientsController()
        {
            clientsHandler = new ClientsHandler();
        }

        [HttpGet]
        public List<ClientModel> Get()
        {
            var clients = clientsHandler.ObtenerClientes();
            return clients;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RegisterUser(ClientModel client) {
            try {
                if (client == null) {
                    return BadRequest();
                }

                ClientsHandler clientsHandler = new ClientsHandler();
                var result = clientsHandler.RegisterUser(client);
                return new JsonResult(result);
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating user");
            }
        }

        [HttpGet("~/api/Login")]
        public List<ClientModel> Login(string UserName, string UserPassword)
        {
            var userLogin = clientsHandler.Authenticate(UserName, UserPassword);
            return userLogin;
        }
    }
}
