using Casino.BLL;
using Microsoft.AspNetCore.Mvc;
using Casino.BLL.DTO;
using Casino.BLL.Authentication;
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
        public ActionResult BandytaInfo(int id)
        {
            return Ok(_Game.GetBanditInfo(id));
        }
        [HttpPost("PlayBandit/{pos1}/{pos2}/{pos3}/{amoutn}")]
        public ActionResult BandytaInfo(int pos1,int pos2,int pos3,int amoutn, [FromBody] UserTokenResponse token)
        {

            return Ok(_Game.PlayBandit(token,new BanditRequestDTO {pos1=pos1,pos2=pos2,pos3=pos3,betAmount=amoutn }));
        }
        [HttpPost("PlayRoullete/{betnum}/{red}/{black}/{amoutn}/{roll}")]
        public ActionResult RoulletePlay(int betnum, bool red, bool black, int amoutn,int roll, [FromBody] UserTokenResponse token)
        {

            return Ok(_Game.PlayRoulette(token, new RouletteRequestDTO { roll=roll, red= red, black= black, betAmount= amoutn, betNumber =betnum }));
        }

        [HttpGet("Roulette/{id}")]
        public ActionResult RouletteInfo(int id)
        {
            return Ok(_Game.GetRouletteInfo(id));
        }
        [HttpGet("Game/{id}")]
        public ActionResult GameInfo(int id)
        {
            return Ok(_Game.GetGameInfo(id));
        }
    }
}
