using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Model.DataTypes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Casino.Model
{
    public class Roulette
    {
        [Key]
        public int RouletteId { get; set; }
        public int GameId { get; set; }
     
        public string Description { get; set; }
        public bool Red {  get; set; }
        public bool Black {  get; set; }
        public int number{  get; set; }
        public int betnumber{  get; set; }
        
        public void Configure(EntityTypeBuilder<Roulette> builder)
        {
           
        }

    }
}
