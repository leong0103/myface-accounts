using Microsoft.AspNetCore.Mvc;
using MyFace.Models.Request;
using MyFace.Models.Response;
using MyFace.Repositories;


namespace MyFace.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersRepo _users;
        public LoginController(IUsersRepo users)
        {
            _users = users;
        }

        [HttpGet("")]
        public bool TryToLogIn([FromBody] string newLoginRequest)
        {
            if(_users.IsValidAccount(newLoginRequest))
            {
                return true;
            }
            return false;
            
        }
    }
}
