using RestSharp;
using System;

namespace OpenSubtitlesComApi
{
    public class InfosApi
    {
        private readonly Api api;

        public InfosApi(Api api)
        {
            this.api = api;
            this.api.EnsureLogin(); //Throws AuthenticationFailureException if unable to login.
        }

        public void RequestUserInfo()
        {
            api.CheckUserToken();
            RestClient client = api.GetRestClient();

            var request = new RestRequest("/infos/user", Method.GET);
            request.AddHeader("Api-Key", api.ApiKey);
            request.AddHeader("Authorization", api.User.token);

            var response = client.Execute(request);

            api.UserData = api.GetJsonNet().Deserialize<UserInfosResponse>(response);
            api.LastRemainingDownloadsCheck = DateTime.UtcNow;
        }
    }
}