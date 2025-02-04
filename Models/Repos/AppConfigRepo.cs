using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Rownd
{
    public class AppConfigRepo
    {
        private static AppConfigRepo instance;
        public static AppConfigRepo Instance => instance ??= new AppConfigRepo();
        private string ConfigUrl = $"{Config.Instance.ApiUrl}/hub/app-config";
        public AppState State { get; private set; }

        private AppConfigRepo() { }

        public async Task<bool> LoadAppConfig(string rowndAppKey, string metaToken)
        {
            Debug.Log("RowndSDK: Fetching App Configuration...");

            UnityWebRequest getRequest = UnityWebRequest.Get(ConfigUrl);
            getRequest.SetRequestHeader("x-rownd-app-key", rowndAppKey);

            TaskCompletionSource<bool> configTask = new TaskCompletionSource<bool>();

            getRequest.SendWebRequest().completed += async operation =>
            {
                if (getRequest.result == UnityWebRequest.Result.Success)
                {
                    string response = getRequest.downloadHandler.text;
                    State = JsonConvert.DeserializeObject<AppState>(response);
                    await StateRepo.Instance.UpdateAppConfigState();
                    configTask.SetResult(true);

                    Debug.Log("RowndSDK: Successfully loaded AppState");
                }
                else
                {
                    Debug.LogError("RowndSDK: API Server config request failed - " + getRequest.error);
                    configTask.SetResult(false);
                }
            };

            bool configResult = await configTask.Task;
            if (configResult)
            {
                await AuthRepo.Instance.LoadAuthState(State.App.Id, metaToken);
            }
            return configResult;
        }
    }
}
