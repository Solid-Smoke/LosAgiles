using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using back_end.Models;
using back_end.Handlers;

namespace back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly ApplicationHandler handler;

        public PaisesController()
        {
            handler = new ApplicationHandler();
        }

        [HttpGet("[action]/details")]
        public List<Person> getPersons(int count)
        {
            return handler.getPersons(count);
        }
    }
}
