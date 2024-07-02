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
    public class UserResponseDTO
    {
        public int UserId { get; set; }
      //  public List<TransactionsResponseDTO>? Transactions { get; set; }
      //  public List<ResultResponseDTO>? Results { get; set; }
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Avatar { get; set; }
        public int Credits { get; set; }
        public UserType UserType { get; set; }
    }
}
