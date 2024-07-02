using AutoMapper;
using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.DAL;
using Casino.Model;
using Casino.Model.DataTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL_EF
{
    public class ResultEF : IResults
    {
        public ResultEF(CasinoDbContext dbContext, UserEF use, IMapper map)
        {
            _context = dbContext;
            this.use = use;
            mapper = map;
        }
        public UserEF use;
        public CasinoDbContext _context;
        public IMapper mapper;

        public async Task<ResultResponseDTO> GetResult(ResultRequestDTO result, UserTokenResponse token)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow || (user.UserId != result.UserId && user.UserType != Model.DataTypes.UserType.Admin))
            {
                throw new SecurityTokenException();
            }

            var xd = await _context.Results.FirstOrDefaultAsync(x => x.UserId == result.UserId && x.GameId == result.GameId);
            return xd == null ? null : mapper.Map<ResultResponseDTO>(xd);
        }

        public async Task<List<ResultResponseDTO>> GetAllUserResults(UserTokenResponse token)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }

            var xd = await _context.Results.Where(x => x.UserId == user.UserId).ToListAsync();
            return xd == null ? null : mapper.Map<List<ResultResponseDTO>>(xd);
        }

        public async Task<List<ResultResponseDTO>> GetAllGameResults(ResultRequestDTO result, UserTokenResponse token)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow || (user.UserId != result.UserId && user.UserType != Model.DataTypes.UserType.Admin))
            {
                throw new SecurityTokenException();
            }

            var xd = await _context.Results.Where(x => x.GameId == result.GameId).ToListAsync();
            return xd == null ? null : mapper.Map<List<ResultResponseDTO>>(xd);
        }

        public async Task<List<ResultResponseDTO>> GetAllUserResults(UserTokenResponse token, int id)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow || user.UserType != UserType.Admin)
            {
                throw new SecurityTokenException();
            }

            var xd = await _context.Results.Where(x => x.UserId == id).ToListAsync();
            return xd == null ? null : mapper.Map<List<ResultResponseDTO>>(xd);
        }
    }
}
