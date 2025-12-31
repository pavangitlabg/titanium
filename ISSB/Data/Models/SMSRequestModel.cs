using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Data.Models
{

    public class SMSRequestModel
    {
        [JsonPropertyName("messages")]
        public List<SMSMessage> messages { get; set; }
    }

    public class SMSMessage
    {
        [JsonPropertyName("from")]
        public string from { get; set; }
        [JsonPropertyName("to")]
        public string to { get; set; }
        [JsonPropertyName("source")]
        public string source { get; set; }
        [JsonPropertyName("body")]
        public string body { get; set; }
    }
}


