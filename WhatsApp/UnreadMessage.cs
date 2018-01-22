using Newtonsoft.Json;
using System.Collections.Generic;

namespace WhatsApp
{
    public class UnreadMessage
    {
        //[JsonProperty("contact")]
        //public ContactModel Contact { get; set; }
        [JsonProperty("messages")]
        public List<MessageModel> Messages { get; set; }
    }
}
