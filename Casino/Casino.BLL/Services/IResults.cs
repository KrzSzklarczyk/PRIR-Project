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
        public Task<ResultResponseDTO> GetResult(ResultRequestDTO result, UserTokenResponse token);
        public Task<List<ResultResponseDTO>> GetAllUserResults( UserTokenResponse token);
        public Task<List<ResultResponseDTO>> GetAllUserResults( UserTokenResponse token,int id);
        public Task<List<ResultResponseDTO>> GetAllGameResults(ResultRequestDTO result, UserTokenResponse token);
    }
}
