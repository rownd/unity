using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Oculus.Platform.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace Rownd
{
    public class UserRepo
    {
        private static UserRepo instance;
        public static UserRepo Instance => instance ??= new UserRepo();
        public UserState State { get; private set; }

        private UserRepo() { }

        public async Task<bool> LoadUserState(string appId, string rowndAccessToken)
        {
            Debug.Log("RowndSDK: Fetching User State...");

            string getUrl = $"{Config.Instance.ApiUrl}/me/applications/{appId}/data";
            UnityWebRequest getRequest = UnityWebRequest.Get(getUrl);
            getRequest.SetRequestHeader("Authorization", $"Bearer {rowndAccessToken}");

            TaskCompletionSource<bool> userTask = new TaskCompletionSource<bool>();

            getRequest.SendWebRequest().completed += async operation =>
            {
                if (getRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = getRequest.downloadHandler.text;
                    State = JsonConvert.DeserializeObject<UserState>(response);
                    await StateRepo.Instance.UpdateUserState();
                    userTask.SetResult(true);

                    Debug.Log("RowndSDK: Successfully loaded UserState");
                }
                else
                {
                    Debug.LogError("RowndSDK: API Server profile request failed: " + getRequest.error);
                    userTask.SetResult(false);
                }
            };
            bool userResult = await userTask.Task;
            if (userResult)
            {
                await StateRepo.Instance.Setup();
            }
            return userResult;
        }
    }
}