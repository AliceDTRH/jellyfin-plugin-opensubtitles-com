using RestSharp;
using System;
using System.Threading.Tasks;
using OpenSubtitlesComApi.Exceptions;
using System.Threading;

namespace OpenSubtitlesComApi
{
    public class DownloadApi
    {
        private readonly Api api;

        public string Url { get; private set; }

        public DownloadApi(Api api)
        {
            this.api = api;
            this.api.EnsureLogin(); //Throws AuthenticationFailureException if unable to login.
        }

        public Task<bool> PerformSubtitleDownloadRequest(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be null or whitespace", nameof(id));
            }

            //If lasstdatarequest + 1 hour is later than now
            if (api.LastRemainingDownloadsCheck.AddHours(1).CompareTo(DateTime.UtcNow) > 0)
            {
                api.Infos.RequestUserInfo();
            }

            if (api.UserData.data.remaining_downloads < 1)
            {
                throw new DownloadRateLimitException("Not starting download, not enough downloads left!");
            }

            return InternalPerformSubtitleDownloadRequest(id, cancellationToken);
        }

        private async Task<bool> InternalPerformSubtitleDownloadRequest(string id, System.Threading.CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting request to download subtitle with id {0}!", id);
            RestClient client = api.GetRestClient();
            RestRequest request = new RestRequest("/download", Method.POST);
            request.AddHeader("Api-Key", api.ApiKey);
            request.AddHeader("Authorization", api.User.token);

            request.AddObject(new DownloadRequest
            {
                file_id = id,
                sub_format = "srt",
                cleanup_links = true,
                remove_adds = true
            });

            IRestResponse response = await client.ExecuteAsync(request, cancellationToken);
            DownloadResponse downloadResponse = api.GetJsonNet().Deserialize<DownloadResponse>(response);

            if (string.IsNullOrWhiteSpace(downloadResponse.link)) { return false; }

            Url = downloadResponse.link;

            api.UserData.data.remaining_downloads = downloadResponse.remaining;
            api.LastRemainingDownloadsCheck = DateTime.UtcNow;
            return true;
        }

        public class DownloadRequest
        {
#pragma warning disable IDE1006 // Naming Styles
            public string file_id { get; set; }
            public string sub_format { get; set; }
            public string file_name { get; set; }
            public bool strip_html { get; set; }
            public bool cleanup_links { get; set; }
            public bool remove_adds { get; set; }
            public int in_fps { get; set; }
            public int out_fps { get; set; }
            public int timeshift { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}