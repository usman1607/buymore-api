using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using BuyMoreApi.Application.Payments;
using BuyMoreApi.Application.Payments.Paystack;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuyMoreApi.Infrastructure.Payments
{
    /// <summary>
    /// Thin wrapper over Paystack's REST API. By keeping the HttpClient logic here we avoid leaking REST details upward.
    /// </summary>
    public sealed class PaystackPaymentService : IPaymentService
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly PaystackOptions _options;
        private readonly ILogger<PaystackPaymentService> _logger;

        public PaystackPaymentService(IOptions<PaystackOptions> options, ILogger<PaystackPaymentService> logger)
        {
            //_httpClient = httpClient;
            _options = options.Value;
            _logger = logger;

            ConfigureHttpClient();
        }

        public async Task<PaystackInitializeResponse> InitializeTransactionAsync(PaystackInitializeRequest request, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Initializing Paystack transaction for {Email} with reference {Reference}", request.Email, request.Reference);

            var payload = JsonSerializer.Serialize(request, SerializerOptions);
            using var content = new StringContent(payload, Encoding.UTF8, "application/json");

            using var response = await _httpClient.PostAsync("/transaction/initialize", content, cancellationToken);
            await EnsureSuccessAsync(response);

            var result = await DeserializeAsync<PaystackEnvelope<PaystackInitializeResponse>>(response, cancellationToken);
            return result.Data;
        }

        public async Task<PaystackVerifyResponse> VerifyTransactionAsync(string reference, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Verifying Paystack transaction with reference {Reference}", reference);

            using var response = await _httpClient.GetAsync($"/transaction/verify/{reference}", cancellationToken);
            await EnsureSuccessAsync(response);

            var result = await DeserializeAsync<PaystackEnvelope<PaystackVerifyResponse>>(response, cancellationToken);
            return result.Data;
        }

        private void ConfigureHttpClient()
        {
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.SecretKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Paystack request failed with {(int)response.StatusCode}: {body}");
            }
        }

        private static async Task<T> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var result = await JsonSerializer.DeserializeAsync<T>(stream, SerializerOptions, cancellationToken)
                         ?? throw new InvalidOperationException("Paystack returned an unexpected payload.");
            return result;
        }

        private sealed class PaystackEnvelope<TPayload>
        {
            public bool Status { get; init; }
            public string Message { get; init; } = string.Empty;
            public TPayload Data { get; init; } = default!;
        }
    }
}
