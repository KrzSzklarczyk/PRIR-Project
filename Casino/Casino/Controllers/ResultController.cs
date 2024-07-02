using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Casino.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : Controller
    {
        private readonly IResults _result;

        public ResultController(IResults res)
        {
            this._result = res;
        }

        [HttpPost("GetAllResults/{userID},{gameID}")]
        public async Task<ActionResult> GetResults(int userID, int gameID, [FromBody] UserTokenResponse token)
        {
            var rez = new ResultRequestDTO { UserId = userID, GameId = gameID };
            var odp = await _result.GetResult(rez, token);
            return odp == null ? BadRequest("bledne dane lub brak") : Ok(odp);
        }

        [HttpPost("GetUserResult")]
        public async Task<ActionResult> GetResultsUser([FromBody] UserTokenResponse token)
        {
            var odp = await _result.GetAllUserResults(token);
            return odp == null ? BadRequest("bledne dane lub brak") : Ok(odp);
        }

        [HttpPost("GetUserResult/{id}")]
        public async Task<ActionResult> AdminUserRes([FromBody] UserTokenResponse token, int id)
        {
            var odp = await _result.GetAllUserResults(token, id);
            return odp == null ? BadRequest("bledne dane lub brak") : Ok(odp);
        }

        [HttpPost("GetGameResult/{gameID}")]
        public async Task<ActionResult> GetResultsGame(int gameID, [FromBody] UserTokenResponse token)
        {
            var rez = new ResultRequestDTO { GameId = gameID };
            var odp = await _result.GetAllGameResults(rez, token);
            return odp == null ? BadRequest("bledne dane lub brak") : Ok(odp);
        }
    }
}
