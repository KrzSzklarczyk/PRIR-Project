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
        public Task<GameResponseDTO> GetGameInfo(int gameId);
       
        public Task<BanditResponseDTO> GetBanditInfo(int gameId);
        public Task<RouletteResponseDTO> GetRouletteInfo(int gameId);
        public Task<bool> PlayBandit(UserTokenResponse token, BanditRequestDTO bandit);
        public Task<bool> PlayRoulette(UserTokenResponse token, RouletteRequestDTO roulette);
    }
}
