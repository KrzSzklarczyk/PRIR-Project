using Casino.BLL;
using Microsoft.AspNetCore.Mvc;
using Casino.BLL.DTO;
using Casino.BLL.Authentication;
using System.Threading.Tasks;

namespace Casino.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly IGame _Game;

        public GameController(IGame Game)
        {
            _Game = Game;
        }

        [HttpGet("Bandit/{id}")]
        public async Task<ActionResult> BandytaInfo(int id)
        {
            var result = await _Game.GetBanditInfo(id);
            return Ok(result);
        }

        [HttpPost("PlayBandit/{pos1}/{pos2}/{pos3}/{amoutn}")]
        public async Task<ActionResult> PlayBandit(int pos1, int pos2, int pos3, int amoutn, [FromBody] UserTokenResponse token)
        {
            var result = await _Game.PlayBandit(token, new BanditRequestDTO { pos1 = pos1, pos2 = pos2, pos3 = pos3, betAmount = amoutn });
            return Ok(result);
        }

        [HttpPost("PlayRoullete/{betnum}/{red}/{black}/{amoutn}/{roll}")]
        public async Task<ActionResult> PlayRoulette(int betnum, bool red, bool black, int amoutn, int roll, [FromBody] UserTokenResponse token)
        {
            var result = await _Game.PlayRoulette(token, new RouletteRequestDTO { roll = roll, red = red, black = black, betAmount = amoutn, betNumber = betnum });
            return Ok(result);
        }

        [HttpGet("Roulette/{id}")]
        public async Task<ActionResult> RouletteInfo(int id)
        {
            var result = await _Game.GetRouletteInfo(id);
            return Ok(result);
        }

        [HttpGet("Game/{id}")]
        public async Task<ActionResult> GameInfo(int id)
        {
            var result = await _Game.GetGameInfo(id);
            return Ok(result);
        }
    }
}
