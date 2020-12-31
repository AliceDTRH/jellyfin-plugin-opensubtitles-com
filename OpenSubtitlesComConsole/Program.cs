using System;
using OpenSubtitlesComApi;

namespace OpenSubtitlesComConsole
{
    internal static class Program
    {
        //Configuration is in user.conf
        private static string Apikey = JsonConfig.Config.Global.ApiKey;

        private static void Main(string[] args)
        {
            Api api = new Api(Program.Apikey);

            Console.WriteLine(AuthenticationApi.TryLogin(api, JsonConfig.Config.Global.username, JsonConfig.Config.Global.password));
            //Console.WriteLine(api.user);
            //Console.WriteLine(DiscoverApi.GetMostDownloaded(api));
            foreach (var subtitle in SubtitleApi.Search(api, "episode", "C:\\Users\\alice\\Desktop\\Sofia The First S01E01.mp4", "en").data)
            {
                Console.WriteLine(subtitle);
                Console.WriteLine("Download? Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.WriteLine("Downloading {0}", subtitle.attributes.files[0].file_id.ToString());
                    var download = new DownloadApi(api, subtitle.attributes.files[0].file_id.ToString());
                    if (download.PerformSubtitleDownloadRequest())
                    {
                        Console.WriteLine(download.Url);
                    }
                    break;
                }
            }
        }
    }
}