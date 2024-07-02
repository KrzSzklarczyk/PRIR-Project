using Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class ResultResponseDTO
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime DateTime { get; set; }
        public int Amount { get; set; }
    }
}
