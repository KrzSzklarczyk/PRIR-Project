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
    public class UserRequestDTO
    {

        public string Login { get; set; }
        public string Password { get; set; }
     
    }
}
