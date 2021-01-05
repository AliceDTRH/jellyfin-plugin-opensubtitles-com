using OpenSubtitlesComApi.Model.DiscoverApi;
using RestSharp;
using System;

namespace OpenSubtitlesComApi
{
    public class DiscoverApi
    {
        private readonly Api api;

        public DiscoverApi(Api api)
        {
            this.api = api;
        }

        public MostDownloadedResponse.Root GetMostDownloaded(string languages = null, string mediaType = null)
        {
            string apikey = api.ApiKey;
            RestClient client = api.GetRestClient();

            var request = new RestRequest("discover/most_downloaded", Method.GET);

            if (!string.IsNullOrEmpty(languages))
            {
                request.AddParameter("languages", languages);
            }

            if (!string.IsNullOrEmpty(mediaType))
            {
                request.AddParameter("type", mediaType);
            }

            request.AddHeader("Api-Key", apikey);
            IRestResponse<MostDownloadedResponse.Root> response = client.Get<MostDownloadedResponse.Root>(request);

            return response.Data;
        }
    }
}