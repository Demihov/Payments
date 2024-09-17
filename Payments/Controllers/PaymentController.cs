using Microsoft.AspNetCore.Mvc;
using Payments.Models;
using Payments.Services;

namespace Payments.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICreditCardService _creditCardService;

        public PaymentController(ICreditCardService creditCardService)
        {
            _creditCardService = creditCardService;
        }

        [HttpPost("validate")]
        public IActionResult ValidateCreditCard([FromBody] CreditCard card)
        {
            var result = _creditCardService.ValidateCreditCard(card);
            {
                if (result.IsValid)
                {
                    return Ok(result.CardType.ToString());
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
        }
    }
}
