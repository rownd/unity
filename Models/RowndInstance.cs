using System.Threading.Tasks;
using Oculus.Platform;
using UnityEngine;

namespace Rownd
{
    public class RowndInstance : MonoBehaviour
    {
        private string rowndAppKey;
        private string metaToken;
        public bool IsAuthenticated { get; private set; } = false; //TODO: remove

        private static RowndInstance instance;

        internal StateRepo StateRepo { get; private set; } = StateRepo.Instance;
        public GlobalState State
        {
            get
            {
                return StateRepo.State;
            }
            private set { }
        }

        public static RowndInstance Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject sdkObject = new GameObject("RowndSDK");
                    instance = sdkObject.AddComponent<RowndInstance>();
                    DontDestroyOnLoad(sdkObject);
                }
                return instance;
            }
        }

        private RowndInstance() { }

        public async Task InitializeAsync(string appKey, string apiUrl = null)
        {
            if (!string.IsNullOrEmpty(rowndAppKey)) return;
            if (!string.IsNullOrEmpty(apiUrl))
            {
                Config.Instance.ApiUrl = apiUrl;
            }

            rowndAppKey = appKey;
            Debug.Log("RowndSDK initialized.");

            await StateRepo.Instance.Setup();

            await GetMetaAccessToken();
        }

        private async Task GetMetaAccessToken()
        {
            Debug.Log("RowndSDK: Requesting Meta token...");

            TaskCompletionSource<string> tokenTask = new TaskCompletionSource<string>();

            Users.GetAccessToken().OnComplete(message =>
            {
                if (message.IsError)
                {
                    Debug.LogError("RowndSDK: Failed to get Meta token - " + message.GetError().Message);
                    tokenTask.SetResult(null);
                }
                else
                {
                    metaToken = message.Data;
                    Debug.Log("RowndSDK: Successfully retrieved Meta token");
                    tokenTask.SetResult(metaToken);
                }
            });

            string token = await tokenTask.Task;
            if (token != null)
            {
                await AppConfigRepo.Instance.LoadAppConfig(rowndAppKey, metaToken);
            }
        }
    }
}