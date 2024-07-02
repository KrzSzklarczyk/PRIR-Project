using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class RouletteRequestDTO
    {
        public int betAmount {  get; set; }
        public int betNumber {  get; set; }
        public bool red {  get; set; }
        public bool black { get; set; }
        public int roll { get; set; }
    }
}
