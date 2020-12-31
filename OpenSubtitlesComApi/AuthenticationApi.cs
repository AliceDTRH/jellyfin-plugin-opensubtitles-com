using OpenSubtitlesComApi.Model.DiscoverApi;
using RestSharp;
using RestSharp.Serializers;
using System;
using OpenSubtitlesComApi.Model.AuthenticationApi;

namespace OpenSubtitlesComApi
{
    public static class AuthenticationApi
    {
        public class RequestObject
        {
#pragma warning disable IDE1006 // Naming Styles
            public string username { get; set; }
            public string password { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }

        public static void CheckUserToken(Api api)
        {
            if (api is null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            if (api.user == null || api.user.token == null || api.user.status != 200)
            {
                throw new InvalidOperationException("User is not logged in.");
            }
        }

        public static bool TryLogin(Api api, string username, string password)
        {
            if (api is null)
            {
                throw new ArgumentNullException(nameof(api));
            }

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

            LoginResponse loginResponse = api.GetJsonNetSerializer().Deserialize<LoginResponse>(response);

            if (loginResponse.status == 200 && loginResponse.token != null)
            {
                api.user = loginResponse;
                api.LastDataRequest = DateTime.UtcNow;
                InfosApi.RequestUserInfo(api);
                return true;
            }
            return false;
        }
    }
}