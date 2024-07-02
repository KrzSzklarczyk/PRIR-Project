using AutoMapper;
using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.DAL;
using Casino.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Casino.BLL_EF
{
    public class GameEF : IGame
    {
        public GameEF(CasinoDbContext dbContext, IMapper mapper_, AuthSettings set, UserEF userEF)
        {
            context = dbContext; mapper = mapper_; _authenticationSettings = set; _userEF = userEF;
        }
        private CasinoDbContext context;
        private IMapper mapper;
        private readonly AuthSettings _authenticationSettings;
        private UserEF _userEF;
        

        public GameResponseDTO GetGameInfo(int gameId)
        {
            var xd = context.Games.FirstOrDefault(x => x.GameId == gameId);
            if (xd == null) { return null; }
            return mapper.Map<GameResponseDTO>(xd);
        }

        public BanditResponseDTO GetBanditInfo(int gameId)
        {
            var xd = context.Bandits.FirstOrDefault(x => x.BanditId == gameId);
            if (xd == null) { return null; }
            return mapper.Map<BanditResponseDTO>(xd);
        }

        public RouletteResponseDTO GetRouletteInfo(int gameId)
        {
            var xd = context.Roulettes.FirstOrDefault(x => x.RouletteId == gameId);
            if (xd == null) { return null; }
            return mapper.Map<RouletteResponseDTO>(xd);
        }


        public bool PlayRoulette(UserTokenResponse token, RouletteRequestDTO roulette)
        {
            int[] red = new int[] { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 36, 35, 34 };
            int[] black = new int[] { 2,4,6,8,10,11,13,15,17,20,22,24,26,29,28,31,33,35 };
            if (roulette.betAmount < 25) return false;

            var principal = _userEF.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            if (user.Credits < roulette.betAmount) return false;

            int amo;
            if (roulette.betNumber==roulette.roll&&roulette.roll!=0)
            {
                amo = roulette.betAmount * 35;

            }
            else if(red.Contains(roulette.roll)|| black.Contains(roulette.roll)&&roulette.roll==-1)
            {
                amo= roulette.betAmount ;
            }
            else if (roulette.roll==0&&roulette.roll==roulette.betNumber)
            {
                amo = roulette.betAmount * 100;
            }
            else { amo = -roulette.betAmount; }
            user.Credits += amo;
            context.Users.Update(user);
            Game game = new Game { Description = "test", EndDate = DateTime.UtcNow, MaxBet = user.Credits - amo, MinBet = 25, StartDate = DateTime.UtcNow, amount = roulette.betAmount };
            Roulette roulette1= new Roulette { Description="test", betnumber=roulette.betNumber, Black= roulette.black, number = roulette.roll, Red = roulette.red};
            game.Roulette = roulette1;
            context.Games.Add(game);
            context.Roulettes.Add(roulette1);
            Result result = new Result { Amount = amo, Game = game, User = user, DateTime = DateTime.UtcNow };
            context.Results.Add(result);
            context.SaveChanges();
            return true;
        }

        public bool PlayBandit(UserTokenResponse token, BanditRequestDTO bandit)
        {
            if (bandit.betAmount < 25) return false;

            var principal = _userEF.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            if (user.Credits < bandit.betAmount) return false;

            int amo;
            if (bandit.pos3 == bandit.pos2 && bandit.pos2 == bandit.pos1)
            {
                amo = bandit.betAmount * (bandit.pos1 + 1) * 100;

            }
            else if (bandit.pos3 == bandit.pos2 || bandit.pos2 == bandit.pos1)
            {
                amo = bandit.betAmount * (bandit.pos1 + 1) * 10;
            }
            else { amo = -bandit.betAmount; }
            user.Credits += amo;
            context.Users.Update(user);
            Game game = new Game { Description = "test", EndDate = DateTime.UtcNow, MaxBet = user.Credits - amo, MinBet = 25, StartDate = DateTime.UtcNow, amount = bandit.betAmount };
            Bandit bandit1 = new Bandit { Description = "test", Position1 = bandit.pos1, Position2 = bandit.pos2, Position3 = bandit.pos3 };
            game.Bandit = bandit1;
            context.Games.Add(game);
            context.Bandits.Add(bandit1);
            Result result = new Result { Amount = amo, Game = game, User = user, DateTime = DateTime.UtcNow };
            context.Results.Add(result);
            context.SaveChanges();
            return true;
        }
    }
    }

       
        
    

