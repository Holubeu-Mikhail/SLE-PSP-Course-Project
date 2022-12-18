using Common.Settings;

namespace Client.Settings
{
    public class ClientSettings : SingleSettings
    {
        public string InputPath { get; set; }

        public string OutputPath { get; set; }
    }
}
