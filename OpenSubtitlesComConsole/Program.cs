using System;
using OpenSubtitlesComApi;

namespace OpenSubtitlesComConsole
{
    internal class Program
    {
        private const string Apikey = "eGRrUxS3IKdR2yIxRWU7HIFwnjneqWhV";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Api api = new Api(Program.Apikey);

            RestSharp.RestClient client = api.GetRestClient();
            //RestSharp.RestClient client = api.GetRestClient();

            Console.WriteLine(AuthenticationApi.TryLogin(api, "alicedtrh", "hD7pmWtt79dEh2dQwNa3"));
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