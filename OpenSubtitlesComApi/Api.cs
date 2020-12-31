using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using OpenSubtitlesComApi.Model.AuthenticationApi;

namespace OpenSubtitlesComApi
{
    public class Api
    {
        private RestClient restClient;
        private JsonNetSerializer jsonNet;
        private string BaseUrl;

        public LoginResponse user;
        public DateTime LastDataRequest;
        public DateTime lastRemainingDownloadsCheck;

        public Api(string apiKey, string baseUrl = "https://www.opensubtitles.com/api/v1/")
        {
            ApiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            BaseUrl = baseUrl;
        }

        public string ApiKey { internal get; set; }
        public UserInfosResponse UserData { get; internal set; }

        public RestClient GetRestClient(bool force = false)
        {
            if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri baseUri))
            {
                throw new InvalidOperationException("Invalid baseUrl");
            }

            if (restClient == null || force)
            {
                restClient = new RestClient(baseUri);
                restClient.UseNewtonsoftJson();
            }
            return restClient;
        }

        public JsonNetSerializer GetJsonNetSerializer(bool force = false)
        {
            if (jsonNet == null || force)
            {
                jsonNet = new JsonNetSerializer();
            }
            return jsonNet;
        }
    }
}