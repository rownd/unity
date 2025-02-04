using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Rownd
{
    public class StateRepo
    {
        private static StateRepo instance;
        public static StateRepo Instance => instance ??= new StateRepo();
        private const string STATE_KEY = "rownd_state";
        public GlobalState State { get; private set; } = new GlobalState();

        private StateRepo() { }

        public async Task Setup()
        {
            await LoadState();
        }

        private async Task LoadState()
        {
            try
            {
                if (PlayerPrefs.HasKey(STATE_KEY))
                {
                    string existingStateJsonStr = PlayerPrefs.GetString(STATE_KEY);

                    if (!string.IsNullOrEmpty(existingStateJsonStr))
                    {
                        State = JsonConvert.DeserializeObject<GlobalState>(existingStateJsonStr) ?? new GlobalState();
                        Debug.Log("RowndSDK: Restoring existing state");
                        return;
                    }
                }

                Debug.Log("RowndSDK: No existing state found. Creating a new default state.");
                await SaveState();
            }
            catch (Exception ex)
            {
                Debug.LogError("RowndSDK: Error loading state. Expected on first run. " + ex);
            }
        }


        private async Task SaveState()
        {
            await Task.Run(() =>
            {
                try
                {
                    string stateJson = JsonConvert.SerializeObject(State);
                    Debug.Log("RowndSDK: Saving state to the device");

                    PlayerPrefs.SetString(STATE_KEY, stateJson);
                    PlayerPrefs.Save(); // Ensure the data is written to storage
                }
                catch (Exception ex)
                {
                    Debug.LogError("RowndSDK: Failed to save state. Error: " + ex);
                }
            });
        }

        public async Task UpdateAppConfigState()
        {
            if (AppConfigRepo.Instance.State != null)
            {
                State.IsInitialized = true;
            }

            if (State.AppConfig == AppConfigRepo.Instance.State)
            {
                return;
            }

            State.AppConfig = AppConfigRepo.Instance.State;
            await SaveState();
        }

        public async Task UpdateAuthState()
        {
            if (State.Auth == AuthRepo.Instance.State)
            {
                return;
            }

            State.Auth = AuthRepo.Instance.State;
            await SaveState();
        }

        public async Task UpdateUserState()
        {
            if (State.User == UserRepo.Instance.State)
            {
                return;
            }

            State.User = UserRepo.Instance.State;
            await SaveState();
        }

        public async Task ClearState()
        {
            await Task.Run(() =>
            {
                State = new GlobalState();
                PlayerPrefs.DeleteKey(STATE_KEY);
            });
        }
    }
}