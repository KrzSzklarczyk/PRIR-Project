using Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class TransactionsResponseDTO
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
    }
}
