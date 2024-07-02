using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL
{
    public interface IResults
    {
        public ResultResponseDTO GetResult(ResultRequestDTO result, UserTokenResponse token);
        public List<ResultResponseDTO> GetAllUserResults( UserTokenResponse token);
        public List<ResultResponseDTO> GetAllUserResults( UserTokenResponse token,int id);
        public List<ResultResponseDTO> GetAllGameResults(ResultRequestDTO result, UserTokenResponse token);
    }
}
