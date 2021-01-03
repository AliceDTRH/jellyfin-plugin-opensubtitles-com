using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.OpenSubtitlesCom.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        // store configurable settings your plugin might need

        public string ApiKey { get; set; }
        public string Username { get; set; }
        public string Password { internal get; set; }

        public PluginConfiguration()
        {
            ApiKey = "cGpQwjOeyFO4w8GhWFDgXO2ui7ZhrRqb";
            Username = "";
            Password = "";
        }
    }
}
