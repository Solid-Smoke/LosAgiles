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

        [HttpPost]
        public List<ClientModel> Login(ClientModel client)
        {
            var userLogin = clientsHandler.Authenticate(client.UserName, client.UserPassword);
            return userLogin;
        }
    }
}
