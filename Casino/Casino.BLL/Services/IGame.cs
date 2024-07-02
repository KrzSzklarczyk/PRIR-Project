using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL
{
    public interface IGame
    {
        public GameResponseDTO GetGameInfo(int gameId);
       
        public BanditResponseDTO GetBanditInfo(int gameId);
        public RouletteResponseDTO GetRouletteInfo(int gameId);
        public bool PlayBandit(UserTokenResponse token, BanditRequestDTO bandit);
        public bool PlayRoulette(UserTokenResponse token, RouletteRequestDTO roulette);
    }
}
