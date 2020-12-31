using OpenSubtitlesComApi.Model.DiscoverApi;
using RestSharp;
using System;

namespace OpenSubtitlesComApi
{
    public static class DiscoverApi
    {
        public static MostDownloadedResponse.Root GetMostDownloaded(Api api, string languages, MediaType? mediaType)
        {
            string apikey = api.ApiKey;
            RestClient client = api.GetRestClient();

            var request = new RestRequest("discover/most_downloaded", Method.GET);

            if (!string.IsNullOrEmpty(languages))
            {
                request.AddParameter("languages", languages);
            }

            if (mediaType.HasValue)
            {
                request.AddParameter("type", mediaType.ToString());
            }

            request.AddHeader("Api-Key", apikey);
            IRestResponse<MostDownloadedResponse.Root> response = client.Get<MostDownloadedResponse.Root>(request);

            return response.Data;
        }

        public static MostDownloadedResponse.Root GetMostDownloaded(Api api) => GetMostDownloaded(api, null, null);
    }
}