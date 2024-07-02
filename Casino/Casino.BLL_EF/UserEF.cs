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
        public UserEF(CasinoDbContext dbContext, IMapper mapper_, AuthSettings set) { _context = dbContext; mapper = mapper_; _authenticationSettings = set; }
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

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, expires: expiredDate, signingCredentials: credentials);

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

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.
                Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");

            }
            return principal;
        }
        internal UserTokenResponse CreateToken(User user, bool populateExp)
        {
            var refreshToken = GetRefreshToken();

            user.RefreshToken = refreshToken;

            if (populateExp)
            {
                user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(7);
            }


            _context.Update(user);
            _context.SaveChanges();
            var token = GetToken(user);
            var response = new UserTokenResponse(token, refreshToken);



            return response;
        }
        public UserTokenResponse Login(UserRequestDTO dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == dto.Login && u.Password == dto.Password);

            if (user == null) { return null; }

            return CreateToken(user, true);
        }
        public UserTokenResponse Register(UserRegisterRequestDTO dto)
        {
            var newUser = new User { Credits = 0, Email = dto.Email , Login=dto.Login, NickName= dto.NickName , Password=dto.Password, Avatar=dto.Avatar, UserType=UserType.User};
            if (_context.Users.FirstOrDefault(x => x.Login == dto.Login && x.Password == dto.Password) != null)
                return null;
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return Login(mapper.Map<UserRequestDTO>(newUser));
        }
        public int GetCredits(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            return user.Credits;

        }

        public List<UserResponseDTO>GetAllUsers(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow|| user.UserType!=UserType.Admin)
            {
                throw new SecurityTokenException();
            }
            return mapper.Map<List<UserResponseDTO>> (_context.Users.ToList());
        }
        public UserTokenResponse RefreshToken(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }

            return CreateToken(user, false);
        }

        public UserResponseDTO GetUserInfo(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            return mapper.Map<UserResponseDTO>( user);
        }

        public int GetUserRole(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            if (user.UserType == UserType.Admin)
                return 1;
            return 0;
        }

        public bool RemoveUser(UserTokenResponse token, int Id)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow || user.UserType != UserType.Admin)
            {
                throw new SecurityTokenException();
            }
            var xd = _context.Users.FirstOrDefault(u => u.UserId == Id);
            if (xd == null) { return false; }
            try
            {
                _context.Users.Remove(xd);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool changeavatar(UserTokenResponse token, string avatar)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
           user.Avatar = avatar;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public bool changepassword(UserTokenResponse token, string passwd)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            if (_context.Users.FirstOrDefault(x => x.Login == user.Login && x.Password == passwd) != null) return false;
            user.Password = passwd;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        public bool deleteUser(UserTokenResponse token)
        {
            var principal = GetPrincipalFromExpiredToken(token.AccessToken);
            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null)
            {
                throw new SecurityTokenException("UserId was not found");
            }

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.UtcNow)
            {
                throw new SecurityTokenException();
            }
            
           _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
