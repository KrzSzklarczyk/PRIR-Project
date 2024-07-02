using Casino.Model.DataTypes;
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
    public class RouletteResponseDTO
    {


        public bool Red { get; set; }
        public bool Black { get; set; }
        public int number { get; set; }
        public int betnumber { get; set; }
    }
}
