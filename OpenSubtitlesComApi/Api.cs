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
        internal readonly string username;
        internal readonly string password;

        public Api(string apiKey, string username, string password, string baseUrl = "https://www.opensubtitles.com/api/v1/")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException($"'{nameof(apiKey)}' cannot be null or whitespace", nameof(apiKey));
            }

            ApiKey = apiKey;
            BaseUrl = baseUrl;

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                this.username = username;
                this.password = password;
            }
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
                SetRestClient(new RestClient(baseUri));
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

        private AuthenticationApi _auth;

        public AuthenticationApi Authentication
        {
            get
            {
                if (_auth == null) { _auth = new AuthenticationApi(this); }
                return _auth;
            }
        }

        private DownloadApi _download;

        public DownloadApi Download
        {
            get
            {
                if (_download == null) { _download = new DownloadApi(this); }
                return _download;
            }
        }

        private InfosApi _infos;

        public InfosApi Infos
        {
            get
            {
                if (_infos == null) { _infos = new InfosApi(this); }
                return _infos;
            }
        }

        private DiscoverApi _discover;

        public DiscoverApi Discover
        {
            get
            {
                if (_discover == null) { _discover = new DiscoverApi(this); }
                return _discover;
            }
        }

        private SubtitleApi _subtitle;

        public SubtitleApi Subtitle
        {
            get
            {
                if (_subtitle == null) { _subtitle = new SubtitleApi(this); }
                return _subtitle;
            }
        }

        internal bool CheckUserToken() => User != null && User.token != null && !string.IsNullOrWhiteSpace(User.token) && User.status == 200;

        internal void EnsureLogin()
        {
            if (!CheckUserToken())
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new AuthenticationFailureException("Username and/or password has not been setup.");
                }
                if (!Authentication.TryLogin(username, password))
                {
                    throw new AuthenticationFailureException("Login has failed.");
                }
            }
        }
    }
}