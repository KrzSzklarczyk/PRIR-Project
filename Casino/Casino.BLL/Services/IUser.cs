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
        public Task<UserTokenResponse> Login(UserRequestDTO user);
        public Task<UserTokenResponse> Register(UserRegisterRequestDTO user);
        public Task<int> GetCredits(UserTokenResponse token);
        public Task<UserResponseDTO> GetUserInfo(UserTokenResponse token);
        public Task<bool> ChangeAvatar(UserTokenResponse token,string avatar);
        public Task<bool> ChangePassword(UserTokenResponse token, string passwd);
        public Task<bool> DeleteUser(UserTokenResponse token);
        public Task<UserTokenResponse> RefreshToken(UserTokenResponse token);
        public Task<List<UserResponseDTO>> GetAllUsers(UserTokenResponse token);
        public Task<int> GetUserRole(UserTokenResponse token);
        public Task<bool> RemoveUser(UserTokenResponse token,int Id);
        
    }
}
