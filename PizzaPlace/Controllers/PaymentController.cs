using Core.Models;
using Microsoft.AspNetCore.Mvc;
using PizzaPlace.Controllers;
using Stripe;

public class PaymentController : BaseApiController
{
    [HttpPost("charge")]
    public IActionResult CreateCharge([FromBody] PaymentRequestModel model)
    {
        try
        {
            var options = new ChargeCreateOptions
            {
                Amount = model.Amount,
                Currency = "usd",
                Source = model.Token,
                Description = "Meal payment"
            };

            var service = new ChargeService();
            var charge = service.Create(options);

            return Ok(new { Message = "Payment successful", Charge = charge });
        }
        catch (StripeException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
