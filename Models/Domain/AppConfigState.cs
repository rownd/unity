using Newtonsoft.Json;

namespace Rownd
{
    public class AppState
    {
        [JsonProperty("app")]
        public AppDetails App { get; set; }
    }

    public class AppDetails
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
