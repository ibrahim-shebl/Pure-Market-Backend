using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test.payment;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private static readonly List<PaymentResponseDto> Payments = new();

        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            if (string.IsNullOrEmpty(paymentRequest.CardNumber) || paymentRequest.Amount <= 0)
            {
                return BadRequest(new PaymentResponseDto
                {
                    IsSuccessful = false,
                    Message = "Invalid payment details."
                });
            }

            var transactionId = Guid.NewGuid().ToString();
            var response = new PaymentResponseDto
            {
                Email = paymentRequest.Email,
                Amount = paymentRequest.Amount,
                IsSuccessful = true,
                TransactionId = transactionId,
                Message = "Payment processed successfully."
            };

            Payments.Add(response);

            return Ok(response);
        }

        [HttpGet("list")]
        public IActionResult GetPayments()
        {
            var paymentSummaries = Payments.Select(p => new
            {
                p.Email,
                p.Amount,
                p.TransactionId
            }).ToList();

            return Ok(paymentSummaries);
        }

        [HttpDelete("{transactionId}")]
        public IActionResult DeletePayment(string transactionId)
        {
            var payment = Payments.FirstOrDefault(p => p.TransactionId == transactionId);
            if (payment == null)
            {
                return NotFound(new { Message = "Payment not found." });
            }

            Payments.Remove(payment);
            return Ok(new { Message = "Payment deleted successfully.", TransactionId = transactionId });
        }
    }
}