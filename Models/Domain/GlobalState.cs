namespace Rownd
{
    public class GlobalState
    {
        public bool IsInitialized { get; internal set; } = false;
        public bool IsReady { get; internal set; } = false;

        public AppState AppConfig { get; set; } = new AppState();
        public AuthState Auth { get; set; } = new AuthState();
        public UserState User { get; set; } = new UserState();
    }
}