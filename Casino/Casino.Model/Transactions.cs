using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Model
{
    public class Transactions
    {
        [Key]
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }
        public required DateTime Date { get; set; }
        public required double Amount { get; set; }

        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.Transactions)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}




