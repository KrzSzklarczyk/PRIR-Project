using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactions _transactions;

        public TransactionController(ITransactions t)
        {
            _transactions = t;
        }

       [HttpPost("History")]
        public ActionResult ViewHistory([FromBody]UserTokenResponse user)
        {
            var history = _transactions.GetHistory(user);
            return Ok(history);
        }
        [HttpPost("History/{id}")]
        public ActionResult AdminHistory([FromBody] UserTokenResponse user,int id)
        {
            var history = _transactions.GetHistory(user,id);
            return Ok(history);
        }

        [HttpPost("New/{amount}")]
        public ActionResult AddTransaction(int amount, [FromBody] UserTokenResponse user)
        {
            var transtaction = _transactions.AddTransaction(amount,user);
            return Ok(transtaction);
        }
    }
}
