namespace Rownd
{
    public class Config
    {
        public string ApiUrl { get; set; } = "https://api.rownd.io";

        private static Config instance;
        public static Config Instance => instance ??= new Config();

        private Config() { }
    }
}