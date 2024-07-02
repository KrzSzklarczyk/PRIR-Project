using AutoMapper;
using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.DAL;
using Casino.Model;
using Casino.Model.DataTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL_EF
{
    public class UserEF : IUser
    {
        public UserEF(CasinoDbContext dbContext, IMapper mapper_, AuthSettings set)
        {
            _context = dbContext;
            mapper = mapper_;
            _authenticationSettings = set;
        }

        private CasinoDbContext _context;
        private IMapper mapper;
        private readonly AuthSettings _authenticationSettings;

        internal string GetToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.NickName}"),
                new Claim(ClaimTypes.Role, $"{user.UserType.ToString()}"),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiredDate = DateTime.Now.AddDays(_authenticationSettings.JwtExpiredDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expiredDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        internal string GetRefreshToken()
        {
            var randomNumber = new byte[64];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        internal ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParamters = new TokenValidationParameters()
            {
                ValidIssuer = _authenticationSettings.JwtIssuer,
                ValidAudience = _authenticationSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParamters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        internal async Task<UserTokenResponse> CreateToken(User user, bool populateExp)
        {
            var refreshToken = GetRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            }

            _context.Update(user);
            await _context.SaveChangesAsync();
            var token = GetToken(user);
            var response = new UserTokenResponse(token, refreshToken);

            return response;
        }

        public async Task<UserTokenResponse> Login(UserRequestDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login && u.Password == dto.Password);

            if (user == null) { return null; }

            return await CreateToken(user, true);
        }

        public async Task<UserTokenResponse> Register(UserRegisterRequestDTO dto)
        {
            var newUser = new User
            {
                Credits = 0,
                Email = dto.Email,
                Login = dto.Login,
                NickName = dto.NickName,
                Password = dto.Password,
                Avatar = dto.Avatar,
                UserType = UserType.User
            };

            if (await _context.Users.AnyAsync(x => x.Login == dto.Login && x.Password == dto.Password))
                return null;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return await Login(mapper.Map<UserRequestDTO>(newUser));
        }

        public async Task<int> GetCredits(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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
            return user.Credits;
        }

        public async Task<List<UserResponseDTO>> GetAllUsers(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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
            return mapper.Map<List<UserResponseDTO>>(await _context.Users.ToListAsync());
        }

        public async Task<UserTokenResponse> RefreshToken(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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

            return await CreateToken(user, false);
        }

        public async Task<UserResponseDTO> GetUserInfo(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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
            return mapper.Map<UserResponseDTO>(user);
        }

        public async Task<int> GetUserRole(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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
            return user.UserType == UserType.Admin ? 1 : 0;
        }

        public async Task<bool> RemoveUser(UserTokenResponse token, int Id)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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

            var xd = await _context.Users.FirstOrDefaultAsync(u => u.UserId == Id);
            if (xd == null)
            {
                return false;
            }
            try
            {
                _context.Users.Remove(xd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ChangeAvatar(UserTokenResponse token, string avatar)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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

            user.Avatar = avatar;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePassword(UserTokenResponse token, string passwd)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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

            if (await _context.Users.AnyAsync(x => x.Login == user.Login && x.Password == passwd))
                return false;

            user.Password = passwd;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
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

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
