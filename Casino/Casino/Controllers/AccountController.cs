using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        [HttpPost,Route("Login")]
        public  IActionResult Login([FromBody] UserRequestDTO user)
        {            
            var xd=_User.Login(user);
            if (xd != null) { return Ok(xd); }
            return BadRequest("User nie istnieje");
        
        }


        [HttpPost, Route("register")]
        public IActionResult Register([FromBody] UserRegisterRequestDTO user)
        {
            var xd = _User.Register(user);
            if (xd != null) { return Ok( xd); }
            return BadRequest("Bladne dane albo user juz istnieje");

        }
        [HttpPost("refresh")]
        public ActionResult Refresh([FromBody] UserTokenResponse token)
        {
            var refreshToken = _User.RefreshToken(token);
            return Ok(refreshToken);
        }
        [HttpPost("getCredits")]
        public ActionResult getCredits([FromBody] UserTokenResponse token)
        {
            var refreshToken = _User.GetCredits(token);
            return Ok(refreshToken);
        }
        [HttpPost("getAllUsers")]
        public ActionResult getusers([FromBody] UserTokenResponse token)
        {
            var refreshToken = _User.GetAllUsers(token);
            return Ok(refreshToken);
        }
        [HttpPost("getUserInfo")]
        public ActionResult getuserInfo([FromBody] UserTokenResponse token)
        {
            var refreshToken = _User.GetUserInfo(token);
            return Ok(refreshToken);
        }

        [HttpPost("GetUserRole")]
        public ActionResult GetUserRole([FromBody] UserTokenResponse token)
        {
            var refreshToken = _User.GetUserRole(token);
            return Ok(refreshToken.ToString());
        }
        [HttpPost("RemoveUser/{id}")]
        public ActionResult RemoveUser([FromBody] UserTokenResponse token,int id)
        {
           return Ok(_User.RemoveUser(token,id));
        }
        [HttpPut("ChangePasswd")]
        public ActionResult ChangePasswd([FromBody] UserChangeDTO userChangeDTO)
        {
            return Ok(_User.changepassword(userChangeDTO.token, userChangeDTO.cos));
        }
        [HttpPut("ChangeAvatar")]
        public ActionResult ChangeAvatar([FromBody] UserChangeDTO userChangeDTO)
        {
            return Ok(_User.changeavatar(userChangeDTO.token, userChangeDTO.cos));
        }
        [HttpPut("RemoveAcc")]
        public ActionResult RemoveAcc([FromBody] UserTokenResponse toekn)
        {
            return Ok(_User.deleteUser(toekn));
        }
    }
}
