using RestSharp;
using System;

namespace OpenSubtitlesComApi
{
    public static class InfosApi
    {
        public static void RequestUserInfo(Api api)
        {
            if (api is null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            AuthenticationApi.CheckUserToken(api);
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