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
    public static decimal GetDailyRevenue(DateTime date)
    {
        var startDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);
        var endDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, DateTimeKind.Utc);

        var options = new ChargeListOptions
        {
            Created = new DateRangeOptions
            {
                GreaterThanOrEqual = startDate,
                LessThanOrEqual = endDate
            }
        };

        var service = new ChargeService();
        var charges = service.List(options).ToList();

        decimal totalRevenue = charges.Sum(c => c.Amount / 100m);

        return totalRevenue;
    }

}
