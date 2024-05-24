using Decor_Business.Repository;
using Decor_Business.Repository.IRepository;
using Decor_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace DecorWeb.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderRepository.GetAll());
        }

        [HttpGet("{orderHeaderId}")]
        public async Task<IActionResult> Get(int? orderHeaderId)
        {
            if (orderHeaderId == null || orderHeaderId == 0)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var orderHeader = await _orderRepository.Get(orderHeaderId.Value);
            if (orderHeader == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(orderHeader);
        }
        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody]StripePaymentDTO stripePaymentDTO)
        {
            stripePaymentDTO.Order.OrderHeader.OrderDate = DateTime.Now;
            var result = await _orderRepository.Create(stripePaymentDTO.Order);
            return Ok(result);
        }

        [HttpPost]
        [ActionName("PaymentSuccessfully")]
        public async Task<IActionResult> PaymentSuccessfully([FromBody] OrderHeaderDTO orderHeaderDTO)
        {
            var service = new SessionService();
            var sessionDetails = service.Get(orderHeaderDTO.SessionId);
            if (sessionDetails.PaymentStatus== "paid" )
            {
                var result = await _orderRepository.MarkPaymentSuccesfull(orderHeaderDTO.Id);
                if (result ==null)
                {
                    return BadRequest(new ErrorModelDTO()
                    {
                        ErrorMessage = "can not mark payment as Successfully"
                    });

                }
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
