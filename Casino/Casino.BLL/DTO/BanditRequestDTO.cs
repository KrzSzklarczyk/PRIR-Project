using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class BanditRequestDTO
    {
        public int pos1 { get; set; }
        public int pos2 { get; set; }
        public int pos3 { get; set; }
        public int betAmount { get; set; }
    }
}
