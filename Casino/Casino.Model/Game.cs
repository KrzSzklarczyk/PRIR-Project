using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Model
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        public int? ResultId { get; set; }
        [ForeignKey(nameof(ResultId))]
        public Result? Result { get; set; }
       
        public int? RouletteId { get; set; }
        [ForeignKey(nameof(RouletteId))]
        public Roulette? Roulette{ get; set; }
        public int? BanditId { get; set; }
        [ForeignKey(nameof(BanditId))]
        public Bandit? Bandit { get; set; }
        [MaxLength(500)]
        public required string Description { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required int MaxBet { get; set; }
        public required int MinBet { get; set; }
        public required int amount {  get; set; }

        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasOne(x => x.Result)
                .WithOne(x => x.Game)
                .OnDelete(DeleteBehavior.Cascade);

           

            

            
        }
    }
}
