using RestSharp;
using System;

namespace OpenSubtitlesComApi
{
    public static class InfosApi
    {
        public static void RequestUserInfo(Api api)
        {
            AuthenticationApi.CheckUserToken(api);
            RestClient client = api.GetRestClient();

            var request = new RestRequest("/infos/user", Method.GET);
            request.AddHeader("Api-Key", api.ApiKey);
            request.AddHeader("Authorization", api.user.token);

            var response = client.Execute(request);

            api.UserData = api.GetJsonNetSerializer().Deserialize<UserInfosResponse>(response);
            api.lastRemainingDownloadsCheck = DateTime.UtcNow;
        }
    }
}