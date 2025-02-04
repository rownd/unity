using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Rownd
{
    public class AuthRepo
    {
        private static AuthRepo instance;
        public static AuthRepo Instance => instance ??= new AuthRepo();
        public AuthState State { get; private set; }

        private AuthRepo() { }

        public async Task<bool> LoadAuthState(string appId, string metaToken)
        {
            Debug.Log("RowndSDK: Fetching Auth State...");

            string postUrl = $"{Config.Instance.ApiUrl}/hub/auth/token";
            string jsonPayload = JsonConvert.SerializeObject(new
            {
                access_token_type = "oculus",
                access_token = metaToken,
                app_id = appId
            });

            UnityWebRequest postRequest = new UnityWebRequest(postUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
            postRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            postRequest.downloadHandler = new DownloadHandlerBuffer();
            postRequest.SetRequestHeader("Content-Type", "application/json");

            TaskCompletionSource<bool> authTask = new TaskCompletionSource<bool>();

            postRequest.SendWebRequest().completed += async operation =>
            {
                if (postRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = postRequest.downloadHandler.text;
                    State = JsonConvert.DeserializeObject<AuthState>(response);
                    await StateRepo.Instance.UpdateAuthState();
                    authTask.SetResult(true);

                    Debug.Log("RowndSDK: Token validated successfully");
                }
                else
                {
                    Debug.LogError("RowndSDK: Token validation failed - " + postRequest.error);
                    authTask.SetResult(false);
                }
            };


            bool authResult = await authTask.Task;
            if (authResult)
            {
                await UserRepo.Instance.LoadUserState(appId, State.AccessToken);
            }

            return authResult;
        }
    }
}
