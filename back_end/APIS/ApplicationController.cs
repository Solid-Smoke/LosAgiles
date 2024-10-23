using back_end.Domain;
using back_end.Infraestructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace back_end.APIS
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

        [HttpGet("[action]/details")]
        public async Task<ActionResult<bool>> verifySuperUserId(string encryptedId)
        {
            try
            {
                if (encryptedId == null)
                {
                    return BadRequest();
                }
                try
                {
                    return handler.verifySuperUserId(
                                                Int32.Parse(
                                                    encryptorDecryptor.getRealId(encryptedId)
                                                    )
                                                );
                }
                catch
                {
                    return StatusCode(
                        StatusCodes.Status404NotFound,
                        "Bad userID or not found in super users list"
                    );
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
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


        [HttpGet("[action]/details")]
        public async Task<ActionResult<List<Users>>> getAllUsersData(int offset, int maxRows, string encryptedId)
        {
            try
            {
                bool isSuperUser = false;
                try
                {
                    isSuperUser = handler.verifySuperUserId(
                                                Int32.Parse(
                                                    encryptorDecryptor.getRealId(encryptedId)
                                                    )
                                                );
                }
                catch
                {
                    return StatusCode(
                        StatusCodes.Status404NotFound,
                        "Bad userID or not found in super users list"
                    );
                }

                if (encryptedId == null)
                {
                    return BadRequest();
                }
                else if (!isSuperUser)
                {
                    return StatusCode(
                        StatusCodes.Status404NotFound,
                        "User ID not found in super users list"
                    );
                }
                else if (isSuperUser)
                {
                    return new JsonResult(
                        handler.getAllUsersData(offset, maxRows)
                    );
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
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
