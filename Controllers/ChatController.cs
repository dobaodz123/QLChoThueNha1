using Microsoft.AspNetCore.Mvc;
using QIChoThueNha1.Models;
using System.Text;
using System.Text.Json;

using ChatRequest = QIChoThueNha1.Models.ChatRequest;
using ChatResponse = QIChoThueNha1.Models.ChatResponse;

namespace QlChoThueNha1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(
            IConfiguration config,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            var apiKey = _config["Gemini:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("Chưa cấu hình API Key.");
            }

            var client = _httpClientFactory.CreateClient();

            // OPENROUTER URL
            var url = "https://openrouter.ai/api/v1/chat/completions";

            // HEADER AUTH
            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Add(
                "Authorization",
                $"Bearer {apiKey}");

            client.DefaultRequestHeaders.Add(
                "HTTP-Referer",
                "http://localhost:5000");

            client.DefaultRequestHeaders.Add(
                "X-Title",
                "House Rental AI");

            // MESSAGE
            var newMessage =
                request.Message
                ?? (request.NewMessage as string)
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(newMessage))
            {
                return BadRequest("Tin nhắn không được để trống.");
            }

            // PAYLOAD OPENROUTER
            var payload = new
            {
                model = "mistralai/mistral-7b-instruct:free",

                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = @"
Bạn là AI hỗ trợ website thuê nhà.

Nhiệm vụ:
- Tư vấn thuê nhà
- Gợi ý giá phù hợp
- Gợi ý khu vực
- Hỗ trợ khách thuê
- Trả lời ngắn gọn bằng tiếng Việt
"
                    },

                    new
                    {
                        role = "user",
                        content = newMessage
                    }
                }
            };

            var json = JsonSerializer.Serialize(payload);

            var response = await client.PostAsync(
                url,
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"));

            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, $"Lỗi {(int)response.StatusCode}: {result}");
            }

            using var doc = JsonDocument.Parse(result);

            var aiText =
                doc.RootElement
                   .GetProperty("choices")[0]
                   .GetProperty("message")
                   .GetProperty("content")
                   .GetString();

            return Ok(new ChatResponse
            {
                Text = aiText ?? "AI không phản hồi."
            });
        }
    }
}