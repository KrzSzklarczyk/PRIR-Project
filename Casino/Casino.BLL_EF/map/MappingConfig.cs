using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Casino.BLL.DTO;
using Casino.Model;

namespace Casino.BLL_EF.map
{
public class MappingConfig :Profile
    {
        public MappingConfig() { 
        CreateMap<BanditResponseDTO,Bandit>().ReverseMap();
       
        CreateMap<GameResponseDTO,Game>().ReverseMap();
        CreateMap<ResultResponseDTO, Result>().ReverseMap();
        CreateMap<RouletteResponseDTO, Roulette>().ReverseMap();
        CreateMap<TransactionsResponseDTO, Transactions>().ReverseMap();
        CreateMap<UserResponseDTO, User>().ReverseMap();

          
            CreateMap<GameRequestDTO,Game>().ReverseMap();
        CreateMap<ResultRequestDTO, Result>().ReverseMap();
      
        CreateMap<TransactionsRequestDTO, Transactions>().ReverseMap();
        CreateMap<UserRequestDTO, User>().ReverseMap();
        }
    }
}
