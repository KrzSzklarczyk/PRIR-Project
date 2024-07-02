using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUser _User;

        public AccountController(IUser u)
        {
            _User = u;
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserRequestDTO user)
        {
            var xd = await _User.Login(user);
            if (xd != null) { return Ok(xd); }
            return BadRequest("User does not exist");
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequestDTO user)
        {
            var xd = await _User.Register(user);
            if (xd != null) { return Ok(xd); }
            return BadRequest("Invalid data or user already exists");
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody] UserTokenResponse token)
        {
            var refreshToken = await _User.RefreshToken(token);
            return Ok(refreshToken);
        }

        [HttpPost("getCredits")]
        public async Task<ActionResult> GetCredits([FromBody] UserTokenResponse token)
        {
            var credits = await _User.GetCredits(token);
            return Ok(credits);
        }

        [HttpPost("getAllUsers")]
        public async Task<ActionResult> GetUsers([FromBody] UserTokenResponse token)
        {
            var users = await _User.GetAllUsers(token);
            return Ok(users);
        }

        [HttpPost("getUserInfo")]
        public async Task<ActionResult> GetUserInfo([FromBody] UserTokenResponse token)
        {
            var userInfo = await _User.GetUserInfo(token);
            return Ok(userInfo);
        }

        [HttpPost("GetUserRole")]
        public async Task<ActionResult> GetUserRole([FromBody] UserTokenResponse token)
        {
            var role = await _User.GetUserRole(token);
            return Ok(role.ToString());
        }

        [HttpPost("RemoveUser/{id}")]
        public async Task<ActionResult> RemoveUser([FromBody] UserTokenResponse token, int id)
        {
            var result = await _User.RemoveUser(token, id);
            return Ok(result);
        }

        [HttpPut("ChangePasswd")]
        public async Task<ActionResult> ChangePasswd([FromBody] UserChangeDTO userChangeDTO)
        {
            var result = await _User.ChangePassword(userChangeDTO.token, userChangeDTO.cos);
            return Ok(result);
        }

        [HttpPut("ChangeAvatar")]
        public async Task<ActionResult> ChangeAvatar([FromBody] UserChangeDTO userChangeDTO)
        {
            var result = await _User.ChangeAvatar(userChangeDTO.token, userChangeDTO.cos);
            return Ok(result);
        }

        [HttpPut("RemoveAcc")]
        public async Task<ActionResult> RemoveAcc([FromBody] UserTokenResponse token)
        {
            var result = await _User.DeleteUser(token);
            return Ok(result);
        }
    }
}
