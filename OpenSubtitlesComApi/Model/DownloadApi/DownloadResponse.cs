namespace OpenSubtitlesComApi
{
    public class DownloadResponse
    {
        public string link { get; set; }
        public string fname { get; set; }
        public int requests { get; set; }
        public int allowed { get; set; }
        public int remaining { get; set; }
        public string message { get; set; }
    }
}