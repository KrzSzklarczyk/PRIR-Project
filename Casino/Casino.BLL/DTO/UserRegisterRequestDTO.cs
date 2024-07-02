using Casino.Model.DataTypes;
using Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class UserRegisterRequestDTO
    {
        
        
        public required string Login { get; set; }
        
        public required string Password { get; set; }
       
        public required string Email { get; set; }
       
        public required string NickName { get; set; }
       
        public string Avatar { get; set; }
        
        
    }
}
