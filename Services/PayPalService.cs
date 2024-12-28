using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace CarInsuranceManage.Services
{
    public class PayPalService
    {
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public PayPalService(IConfiguration configuration)
        {
            _configuration = configuration;
            _clientId = _configuration["Paypal:ClientId"];
            _secretKey = _configuration["Paypal:SecretKey"];
            _baseUrl = _configuration["Paypal:BaseUrl"];
        }

        // Step 1: Obtain OAuth Token
        private async Task<string> GetAccessToken()
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("/v1/oauth2/token", Method.Post);

            request.AddHeader("Authorization", $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_secretKey}"))}");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials", ParameterType.GetOrPost);

            var response = await client.ExecutePostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception($"Error obtaining PayPal token: {response.Content}");

            var token = JObject.Parse(response.Content)["access_token"]?.ToString();
            if (token == null)
                throw new Exception("Failed to retrieve access token from PayPal.");

            return token;
        }



        // Step 2: Create Payment
        public async Task<string> CreatePayment(decimal amount, string currency, string description, string returnUrl, string cancelUrl)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0.");

            var token = await GetAccessToken();
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("/v1/payments/payment", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var paymentBody = new
            {
                intent = "sale",
                payer = new { payment_method = "paypal" },
                transactions = new[]
                {
            new
            {
                amount = new
                {
                    total = amount.ToString("F2", CultureInfo.InvariantCulture),
                    currency
                },
                description
            }
        },
                redirect_urls = new
                {
                    return_url = returnUrl,
                    cancel_url = cancelUrl
                }
            };

            request.AddJsonBody(paymentBody);

            var response = await client.ExecutePostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception($"Error creating PayPal payment: {response.Content}");

            var jsonResponse = JObject.Parse(response.Content);
            var approvalUrl = jsonResponse["links"]
                ?.FirstOrDefault(x => x["rel"]?.ToString() == "approval_url")?["href"]?.ToString();

            if (approvalUrl == null)
                throw new Exception("Approval URL not found in PayPal response.");

            return approvalUrl;
        }


        // Step 3: Execute Payment
        public async Task<string> ExecutePayment(string paymentId, string payerId)
        {
            var token = await GetAccessToken();
            var client = new RestClient(_baseUrl);
            var request = new RestRequest($"/v1/payments/payment/{paymentId}/execute", Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddHeader("Content-Type", "application/json");

            var executeBody = new { payer_id = payerId };
            request.AddJsonBody(executeBody);

            var response = await client.ExecutePostAsync(request);
            if (!response.IsSuccessful)
                throw new Exception($"Error executing PayPal payment: {response.Content}");

            return response.Content;
        }

    }
}
