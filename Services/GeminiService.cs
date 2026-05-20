using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace QlChoThueNha1.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiKey = "API_KEY_CUA_BAN";

        public GeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AskAI(string message)
        {
            var url =
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            var body = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = $"Bạn là AI tư vấn thuê nhà. Câu hỏi: {message}"
                            }
                        }
                    }
                }
            };

            var json = JsonConvert.SerializeObject(body);

            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);

            var result = await response.Content.ReadAsStringAsync();

            dynamic data = JsonConvert.DeserializeObject(result);

            return data.candidates[0].content.parts[0].text.ToString();
        }
    }
}