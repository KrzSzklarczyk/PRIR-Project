using Casino.BLL.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class UserChangeDTO
    {
       public UserTokenResponse token {  get; set; }
        public string cos {  get; set; }
    }
}
