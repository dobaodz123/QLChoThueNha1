namespace QIChoThueNha1.Models
{
    public class ChatRequest
    {
        // Thêm dấu ? và đảm bảo tất cả đều là { get; set; }
        public string? Message { get; set; }
        public string? NewMessage { get; set; }
        public List<ChatMessage>? History { get; set; }
    }

    public class ChatMessage
    {
        public string? Role { get; set; } // "user" hoặc "model"
        public string? Text { get; set; }
    }
}