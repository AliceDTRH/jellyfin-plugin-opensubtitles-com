namespace OpenSubtitlesComApi.Model.AuthenticationApi
{
    public class LoginResponse
    {
#pragma warning disable IDE1006 // Naming Styles
        public string token { get; set; }
        public int status { get; set; }
#pragma warning restore IDE1006 // Naming Styles

        public override string ToString()
        {
            return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }
    }
}