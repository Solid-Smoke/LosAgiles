using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using back_end.Handlers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly ApplicationHandler handler;

        private EncryptDecryptUtilities encryptorDecryptor;

        public ShopController()
        {
            handler = new ApplicationHandler();
            encryptorDecryptor = new EncryptDecryptUtilities();
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

        [HttpGet("[action]/details")]        
        public async Task<ActionResult<string>> authSuperUser(string userName, string passwordHash)
        {
            try
            {
                if (userName == null || passwordHash == null)
                {
                    return BadRequest();
                }
                return new JsonResult(encryptorDecryptor.encryptId(handler.authSuperUser(userName, passwordHash)));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
