namespace QlChoThueNha1.Models
{
    public class ChatRequest
    {
        public string Message { get; set; }
        public List<ChatMessage> History { get; set; }
    }
    public class ChatMessage
    {
        public string Role { get; set; } // "user" hoặc "model"
        public string Text { get; set; }
    }
}