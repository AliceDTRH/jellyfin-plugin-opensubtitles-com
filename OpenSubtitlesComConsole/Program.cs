using System;
using JsonConfig;
using OpenSubtitlesComApi;

namespace OpenSubtitlesComConsole
{
    internal static class Program
    {
        //Configuration is in user.conf
        private static readonly string Apikey = JsonConfig.Config.Global.ApiKey;

        private static void Main(string[] args)
        {
            string arg = string.Join(" ", args);
            Api api = new Api(Program.Apikey);

            if (AuthenticationApi.TryLogin(api, Config.Global.Username, Config.Global.Password))
            {
                var search = SubtitleApi.Search(api, "all", arg, "en").Result;
                if (search.data == null) { Console.WriteLine(search); return; }
                foreach (var subtitle in search.data)
                {
                    Console.WriteLine(subtitle);
                    Console.WriteLine("Download? Y/N");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        Console.WriteLine("Downloading {0}", subtitle.attributes.files[0].file_id.ToString());
                        var download = new DownloadApi(api, subtitle.attributes.files[0].file_id.ToString());
                        if (download.PerformSubtitleDownloadRequest(new System.Threading.CancellationToken()).Result)
                        {
                            Console.WriteLine(download.Url);
                        }
                        break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Something went wrong trying to login.");
            }
        }
    }
}