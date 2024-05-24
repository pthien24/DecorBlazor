using Decor_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace DecorWeb.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StripePaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        public StripePaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Authorize]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] StripePaymentDTO stripePaymentDTO)
        {
            try
            {

            
            var domaim = _configuration.GetValue<string>("Decor_client_Url");
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domaim + stripePaymentDTO.SucssesUrl,
                CancelUrl = domaim + stripePaymentDTO.CancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                PaymentMethodTypes = new List<string> { "card"}
            };
            foreach (var item in stripePaymentDTO.Order.OrderDetails)
            {
                var sesionlineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        }
                    },
                    Quantity = item.Count,

                };
                options.LineItems.Add(sesionlineItem);

            }
            var service = new Stripe.Checkout.SessionService();
            Session session =  service.Create(options);
            return Ok(new SuccessModelDTO()
            {
                Data = session.Id,
            });

            }
            catch (Exception ex )
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = ex.Message,
                });
            }
        }
    }
}
