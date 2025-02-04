using Newtonsoft.Json;

namespace Rownd
{
    public class AuthState
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("sign_in_method")]
        public string SignInMethod { get; set; }

        [JsonProperty("user_type")]
        public string UserType { get; set; }

        [JsonIgnore]
        public bool IsAuthenticated
        {
            get { return AccessToken != null; }
        }

        [JsonIgnore]
        public bool IsNotAuthenticated
        {
            get { return AccessToken == null; }
        }
    }
}