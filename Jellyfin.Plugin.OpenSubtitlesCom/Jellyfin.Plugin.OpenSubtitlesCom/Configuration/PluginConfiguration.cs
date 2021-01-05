using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.OpenSubtitlesCom.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        // store configurable settings your plugin might need

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
    }
}
