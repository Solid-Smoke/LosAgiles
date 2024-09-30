using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] ClientModel client) {
            if (client == null) {
                return BadRequest("Invalid login request.");
            }

            ClientModel userLogin = clientsHandler.Authenticate(client.UserName, client.UserPassword);

            if (userLogin == null) {
                return Unauthorized(); // error 401
            }

            //var token = GenerateJwtToken(userLogin);
            var token = "";

            return Ok(new { Token = token });
        }

        //        private string GenerateJwtToken(ClientModel user) {
        //            var claims = new[]
        //            {
        //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //            };

        //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key"));
        //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //            var token = new JwtSecurityToken(
        //                issuer: null,
        //                audience: null,
        //                claims: claims,
        //                signingCredentials: creds);

        //            return new JwtSecurityTokenHandler().WriteToken(token);
        //            }
    }
}
