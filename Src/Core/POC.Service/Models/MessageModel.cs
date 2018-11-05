using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POC.Service.Models
{
    public class MessageModel
    {
        [JsonProperty(PropertyName = "messageId")]
        public string MessageId { get; set; }
        [JsonProperty(PropertyName = "messageDescription")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "messageSeverity")]
        public string Severity { get; set; }
    }
}
