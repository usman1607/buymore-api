using System.Text;
using System.Text.Json;
using BuyMoreApi.Application.Payments;
using BuyMoreApi.Application.Payments.Paystack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyMoreApi.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaystackWebhookService _paystackWebhookService;

        public PaymentsController(IPaymentService paymentService, IPaystackWebhookService paystackWebhookService)
        {
            _paymentService = paymentService;
            _paystackWebhookService = paystackWebhookService;
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> Initialize([FromBody] PaystackInitializeRequest request, CancellationToken cancellationToken)
        {
            var response = await _paymentService.InitializeTransactionAsync(request, cancellationToken);
            return Ok(response);
        }

        [HttpGet("verify/{reference}")]
        public async Task<IActionResult> Verify([FromRoute] string reference, CancellationToken cancellationToken)
        {
            var response = await _paymentService.VerifyTransactionAsync(reference, cancellationToken);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("paystack/webhook")]
        public async Task<IActionResult> PaystackWebhook(CancellationToken cancellationToken)
        {
            // Paystack sends the signature in a header; we re-read the raw body so the signature check stays faithful.
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            var signature = Request.Headers["x-paystack-signature"].ToString();
            using var payload = JsonDocument.Parse(rawBody);

            await _paystackWebhookService.HandleAsync(payload, signature, rawBody, cancellationToken);
            return Ok();
        }
    }
}
