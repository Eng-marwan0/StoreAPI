using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace StoreAPI.Services
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _http;

        public WhatsAppService(IConfiguration config)
        {
            _config = config;

            // حل مشكلة SSL و Timeout
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };

            _http = new HttpClient(handler);
            _http.Timeout = TimeSpan.FromSeconds(15);
        }


        public async Task<bool> SendOtpAsync(string phone, string code)
        {
            string token = _config["WhatsApp:AccessToken"];
            string phoneId = _config["WhatsApp:PhoneNumberId"];

            var url = $"https://graph.facebook.com/v18.0/{phoneId}/messages";

            // نُجبر JSON على الخروج بصيغة camelCase
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var payload = new
            {
                messaging_product = "whatsapp",
                to = phone,
                type = "text",
                text = new
                {
                    body = $"🔐 رمز التحقق الخاص بك هو: {code}"
                }
            };

            var json = JsonSerializer.Serialize(payload, options);

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();

            Console.WriteLine("===== WHATSAPP API RESPONSE =====");
            Console.WriteLine(responseText);
            Console.WriteLine("================================");

            return response.IsSuccessStatusCode;
        }

    }
}