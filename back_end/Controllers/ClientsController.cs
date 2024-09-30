using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("~/api/Login")]
        public List<ClientModel> Login(string client, string client2)
        {
            var userLogin = clientsHandler.Authenticate(client, client2);
            return userLogin;
        }
    }
}
