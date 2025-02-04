using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rownd
{
    public class UserState
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("meta")]
        public MetaData Meta { get; set; }

    }
    public class Data
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("oculus_id")]
        public string OculusId { get; set; }
    }

    public class MetaData
    {
        [JsonProperty("created")]
        public string Created { get; set; }
    }
}
