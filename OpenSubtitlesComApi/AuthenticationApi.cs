using RestSharp;
using System;
using OpenSubtitlesComApi.Model.AuthenticationApi;

namespace OpenSubtitlesComApi
{
    public class AuthenticationApi
    {
        public class RequestObject
        {
#pragma warning disable IDE1006 // Naming Styles
            public string username { get; set; }
            public string password { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }

        private readonly Api api;

        public AuthenticationApi(Api api)
        {
            this.api = api;
        }

        public bool TryLogin(string username, string password)
        {
            RestClient client = api.GetRestClient();

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty", nameof(password));
            }

            var request = new RestRequest("/login", Method.POST);

            request.AddHeader("Api-Key", api.ApiKey);

            request.AddObject(new RequestObject
            {
                username = username,
                password = password
            });

            IRestResponse response = client.Execute(request);

            LoginResponse loginResponse = api.GetJsonNet().Deserialize<LoginResponse>(response);

            if (loginResponse.status == 200 && loginResponse.token != null)
            {
                api.User = loginResponse;
                api.LastDataRequest = DateTime.UtcNow;
                api.Infos.RequestUserInfo();
                return api.CheckUserToken();
            }
            return false;
        }
    }
}