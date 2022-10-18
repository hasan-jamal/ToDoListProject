using MessagesProject.Core.Managers.Interfaces;
using MessagesProject.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MessagesProject.Controllers
{
    [ApiController]
    public class UserController : ApiBaseController
    {
        private IUserManager _userManager;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger,
                              IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/<UserController>

        [Route("api/user/signUp")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult SignUp([FromBody] UserRegistrationModel userReg)
        {
            var res = _userManager.SignUp(userReg);
            return Ok(res);
        }

        [Route("api/user/login")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginModelView userReg)
        {
            var res = _userManager.Login(userReg);
            return Ok(res);
        }

        // PUT api/<UserController>/5
        // update my profile
        [Route("api/user/UpdateMyProfile")]
        [HttpPut]
        [Authorize]
        public IActionResult UpdateMyProfile(UserModel request)
        {
            var user = _userManager.UpdateProfile(LoggedInUser , request);
            return Ok(user);
        }

        // DELETE api/<UserController>/5
        [HttpDelete]
        [Route("api/user/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _userManager.DeleteUser(LoggedInUser, id);
            return Ok();
        }
    }
}
