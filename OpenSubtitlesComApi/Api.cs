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
        private readonly string BaseUrl;
        internal readonly string ApiKey;

        public Api(string apiKey, string baseUrl = "https://www.opensubtitles.com/api/v1/")
        {
            ApiKey = apiKey;
            BaseUrl = baseUrl;
        }

        public UserInfosResponse UserData { get; internal set; }
        public LoginResponse User { get; internal set; }
        public DateTime LastDataRequest { get; internal set; }
        public DateTime LastRemainingDownloadsCheck { get; internal set; }

        public RestClient GetRestClient(bool recreate = false)
        {
            if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri baseUri))
            {
                throw new InvalidOperationException("Invalid baseUrl");
            }

            if (restClient == null || recreate)
            {
                restClient = new RestClient(baseUri);
                restClient.UseNewtonsoftJson();
            }
            return restClient;
        }

        public void SetRestClient(RestClient value)
        {
            restClient = value;
            restClient.UseNewtonsoftJson();
        }

        public JsonNetSerializer GetJsonNet(bool recreate = false)
        {
            if (jsonNet == null || recreate)
            {
                SetJsonNet(new JsonNetSerializer());
            }

            return jsonNet;
        }

        public void SetJsonNet(JsonNetSerializer value)
        {
            jsonNet = value;
        }
    }
}