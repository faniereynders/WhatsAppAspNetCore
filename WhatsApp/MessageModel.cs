using Newtonsoft.Json;

namespace WhatsApp
{
    public class MessageModel
    {
        [JsonProperty("message")]
        public string Body { get; set; }
        [JsonProperty("timestamp")]
        public int Timestamp { get; set; }
        [JsonProperty("contact")]
        public ContactModel Contact { get; set; }
    }
}
