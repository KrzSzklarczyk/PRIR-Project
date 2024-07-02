using AutoMapper;
using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.DAL;
using Casino.Model;
using Casino.Model.DataTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Casino.BLL_EF
{
    public class TransactionsEF : ITransactions
    {
        public TransactionsEF(CasinoDbContext dbContext, UserEF use, IMapper map)
        {
            _context = dbContext;
            this.use = use;
            mapper = map;
        }
        public UserEF use;
        public CasinoDbContext _context;
        public IMapper mapper;

        public async Task<List<TransactionsResponseDTO>> GetHistory(UserTokenResponse token)
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

            var wyn = await _context.Transactions.Where(x => x.UserId == user.UserId).ToListAsync();
            return wyn == null ? null : mapper.Map<List<TransactionsResponseDTO>>(wyn);
        }

        public async Task<bool> AddTransaction(int amount, UserTokenResponse token)
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

            var transaction = new Transactions { Amount = amount, Date = DateTime.UtcNow, User = user };
            user.Credits += amount;
            _context.Users.Update(user);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TransactionsResponseDTO>> GetHistory(UserTokenResponse token, int id)
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

            var wyn = await _context.Transactions.Where(x => x.UserId == id).ToListAsync();
            return wyn == null ? null : mapper.Map<List<TransactionsResponseDTO>>(wyn);
        }
    }
}
