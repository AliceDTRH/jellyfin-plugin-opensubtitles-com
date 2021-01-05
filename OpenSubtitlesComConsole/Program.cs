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
            Api api = new Api(Program.Apikey, Config.Global.Username, Config.Global.Password);

            var search = api.Subtitle.Search("all", arg, "en").Result;
            if (search.data == null) { Console.WriteLine(search); return; }
            foreach (var subtitle in search.data)
            {
                Console.WriteLine(subtitle);
                Console.WriteLine("Download? Y/N");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    string subtitle_id = subtitle.attributes.files[0].file_id.ToString();
                    Console.WriteLine("Downloading {0}", subtitle_id);

                    if (api.Download.PerformSubtitleDownloadRequest(subtitle_id, new System.Threading.CancellationToken()).Result)
                    {
                        Console.WriteLine(api.Download.Url);
                    }
                    break;
                }
            }
        }
    }
}