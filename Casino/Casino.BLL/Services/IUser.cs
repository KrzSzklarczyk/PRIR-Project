using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Casino.Model.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL
{
    public interface IUser
    {
        public UserTokenResponse Login(UserRequestDTO user);
        public UserTokenResponse Register(UserRegisterRequestDTO user);
        public int GetCredits(UserTokenResponse token);
        public UserResponseDTO GetUserInfo(UserTokenResponse token);
        public bool changeavatar(UserTokenResponse token,string avatar);
        public bool changepassword(UserTokenResponse token, string passwd);
        public bool deleteUser(UserTokenResponse token);
        public UserTokenResponse RefreshToken(UserTokenResponse token);
        public List<UserResponseDTO> GetAllUsers(UserTokenResponse token);
        public int GetUserRole(UserTokenResponse token);
        public bool RemoveUser(UserTokenResponse token,int Id);
        
    }
}
