using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSubtitlesComApi.Model.AuthenticationApi
{
    public class LoginResponse
    {
        public string token;
        public int status;

        public override string ToString()
        {
            return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }
    }
}