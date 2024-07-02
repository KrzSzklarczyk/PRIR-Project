using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Model.DataTypes;

namespace Casino.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public  List<Transactions>? Transactions { get; set; }
        public  List<Result>? Results { get; set; }
        [MaxLength(30)]
        public required string Login { get; set; }
        [MaxLength(30)]
        public required string Password { get; set; }
        [MaxLength(30)]
        public required string Email { get; set; }
        [MaxLength(30)]
        public required string NickName { get; set; }
        public required int Credits { get; set; }
        public UserType UserType { get; set; }
        public string Avatar {  get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
