using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.Authentication
{
    public record UserTokenResponse(string AccessToken, string RefreshToken)
    {
        public string AccessToken { get; set; } = AccessToken;
        public string RefreshToken { get; set; } = RefreshToken;
    }
}
