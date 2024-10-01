using back_end.Handlers;
using back_end.Models;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SuperUsersController : ControllerBase {
        private readonly SuperUsersHandler superUsersHandler;

        public SuperUsersController() {
            superUsersHandler = new SuperUsersHandler();
        }

        [HttpGet("~/api/SuperUserLogin")]
        public List<SuperUserModel> Login(string UserName, string UserPassword) {
            var superUsersLogin = superUsersHandler.Authenticate(UserName, UserPassword);
            return superUsersLogin;
        }
    }
}
