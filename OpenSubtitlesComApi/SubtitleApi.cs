using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenSubtitlesComApi
{
    public static class SubtitleApi
    {
        public class SubtitleRequest
        {
            public string moviehash { get; set; }
            public string query { get; set; }
            public string type { get; set; }
            public string languages { get; set; }
        }

        public static SubtitleResponse Search(Api api, string type, string filepath, string languages)
        {
            if (api is null)
            {
                throw new ArgumentNullException(nameof(api));
            }

            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException($"'{nameof(type)}' cannot be null or empty", nameof(type));
            }

            if (string.IsNullOrEmpty(filepath))
            {
                throw new ArgumentException($"'{nameof(filepath)}' cannot be null or empty", nameof(filepath));
            }

            if (string.IsNullOrEmpty(languages))
            {
                throw new ArgumentException($"'{nameof(languages)}' cannot be null or empty", nameof(languages));
            }

            if (!File.Exists(filepath)) { throw new FileNotFoundException($"{filepath} was not found!", filepath); }
            string filename = Path.GetFileName(filepath);
            string hash = Util.GetHash(filepath);
            var request = new RestRequest("/subtitles", Method.GET);
            request.AddHeader("Api-Key", api.ApiKey);

            request.AddObject(new SubtitleRequest
            {
                moviehash = hash,
                query = filename,
                type = type.ToString(),
                languages = languages
            });

            RestClient client = api.GetRestClient();
            var response = client.Execute(request);

            return api.GetJsonNetSerializer().Deserialize<SubtitleResponse>(response);
        }
    }
}