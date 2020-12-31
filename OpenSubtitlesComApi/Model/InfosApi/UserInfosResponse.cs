namespace OpenSubtitlesComApi
{
    public class UserInfosResponse
    {
        public Data data { get; set; }

        public class Data
        {
            public int allowed_downloads { get; set; }
            public string level { get; set; }
            public int user_id { get; set; }
            public bool ext_installed { get; set; }
            public bool vip { get; set; }
            public int downloads_count { get; set; }
            public int remaining_downloads { get; set; }
        }
    }
}