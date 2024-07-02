using Casino.BLL;
using Casino.BLL.Authentication;
using Casino.BLL.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<ActionResult> ViewHistory([FromBody] UserTokenResponse user)
        {
            var history = await _transactions.GetHistory(user);
            return Ok(history);
        }

        [HttpPost("History/{id}")]
        public async Task<ActionResult> AdminHistory([FromBody] UserTokenResponse user, int id)
        {
            var history = await _transactions.GetHistory(user, id);
            return Ok(history);
        }

        [HttpPost("New/{amount}")]
        public async Task<ActionResult> AddTransaction(int amount, [FromBody] UserTokenResponse user)
        {
            var transaction = await _transactions.AddTransaction(amount, user);
            return Ok(transaction);
        }
    }
}
