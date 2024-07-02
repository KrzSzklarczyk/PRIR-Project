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
    public class Bandit
    {
        [Key]
        public int BanditId { get; set; }
        
        public required string Description { get; set; }
        public required int Position1 { get; set; }
        public required int Position2 { get; set; }
        public required int Position3 { get; set; }

        public void Configure(EntityTypeBuilder<Bandit> builder)
        {
           
        }

    }
}
