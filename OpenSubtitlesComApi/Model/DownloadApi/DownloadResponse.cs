namespace OpenSubtitlesComApi
{
    public class DownloadResponse
    {
#pragma warning disable IDE1006 // Naming Styles
        public string link { get; set; }
        public string fname { get; set; }
        public int requests { get; set; }
        public int allowed { get; set; }
        public int remaining { get; set; }
        public string message { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}