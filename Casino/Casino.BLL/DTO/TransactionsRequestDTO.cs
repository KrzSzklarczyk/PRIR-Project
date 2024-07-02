using Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.BLL.DTO
{
    public class TransactionsRequestDTO
    {
        public int? TransactionId { get; set; }
        public int? UserId { get; set; }

    }
}
