using Microsoft.AspNetCore.Mvc;
using TelrPayment.Models.Telr.SendRequest;
using TelrPayment.Services;

namespace TelrPayment.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("SendPaymentRequest")]
        public async Task<IActionResult> SendPaymentRequest(int orderId)
        {
            var result = await _paymentService.SendPaymentRequest(orderId);
            if (!result.HasErrors)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpPost("CheckPaymentRequest")]
        public async Task<IActionResult> CheckPaymentRequest(int orderId)
        {
            var result = await _paymentService.CheckPaymentRequest(orderId);
            if (!result.HasErrors)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

    }
}
