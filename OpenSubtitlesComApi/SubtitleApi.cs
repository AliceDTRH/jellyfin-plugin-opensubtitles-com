using RestSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OpenSubtitlesComApi
{
    public class SubtitleApi
    {
        private readonly Api api;

        public SubtitleApi(Api api)
        {
            this.api = api;
        }

        private class SubtitleRequest
        {
#pragma warning disable IDE1006 // Naming Styles
            public string moviehash { get; set; }
            public string query { get; set; }
            public string type { get; set; }
            public string languages { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }

        public Task<SubtitleResponse> Search(string type, string filepath, string languages)
        {
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

            return SearchInternal(type, filepath, languages);
        }

        public async Task<SubtitleResponse> SearchInternal(string type, string filepath, string languages)
        {
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
                languages = languages,
            });

            RestClient client = api.GetRestClient();
            var response = await client.ExecuteAsync(request);

            return api.GetJsonNet().Deserialize<SubtitleResponse>(response);
        }
    }
}