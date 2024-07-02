using Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class GameResponseDTO
    {
        
        public int ResultId { get; set; }
        public int? BlackJackId { get; set; }
        public int? RouletteId { get; set; }
        public int? BanditId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxBet { get; set; }
        public int MinBet { get; set; }
        public  int amount { get; set; }
    }
}
