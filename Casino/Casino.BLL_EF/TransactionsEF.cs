using AutoMapper;
using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.DAL;
using Casino.Model;
using Casino.Model.DataTypes;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

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

        public List<TransactionsResponseDTO> GetHistory( UserTokenResponse token)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow )
            {
                throw new SecurityTokenException();
            }
            var wyn= _context.Transactions.Where(x=>x.UserId== user.UserId).ToList();
            return wyn == null ? null : mapper.Map<List<TransactionsResponseDTO>>(wyn);

        }

        public bool AddTransaction(int amount,  UserTokenResponse token)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow )
            {
                throw new SecurityTokenException();
            }
            var kek = new Transactions { Amount = amount, Date = DateTime.UtcNow, User = user };
            user.Credits += amount;
            _context.Users.Update(user);
            Transactions transactions = new Transactions { Amount = amount, Date = DateTime.UtcNow, User = user };
            _context.Transactions.Add(transactions);
            _context.SaveChanges();
            return true;
            

        }

        public List<TransactionsResponseDTO> GetHistory(UserTokenResponse token, int id)
        {
            var principal = use.GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow||user.UserType!=UserType.Admin)
            {
                throw new SecurityTokenException();
            }
            var wyn = _context.Transactions.Where(x => x.UserId == id).ToList();
            return wyn == null ? null : mapper.Map<List<TransactionsResponseDTO>>(wyn);
        }
    }
}
