using System;

namespace OpenSubtitlesComApi
{
#pragma warning disable IDE1006 // Naming Styles - From JSON Api

    public class SubtitleResponse
    {
        public int total_pages { get; set; }
        public int total_count { get; set; }
        public int page { get; set; }
        public Datum[] data { get; set; }

        public class Datum
        {
            public string id { get; set; }
            public string type { get; set; }
            public Attributes attributes { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public class Attributes
        {
            public string subtitle_id { get; set; }
            public string language { get; set; }
            public int download_count { get; set; }
            public int new_download_count { get; set; }
            public bool hearing_impaired { get; set; }
            public bool hd { get; set; }
            public string format { get; set; }
            public float fps { get; set; }
            public int votes { get; set; }
            public int points { get; set; }
            public float ratings { get; set; }
            public bool from_trusted { get; set; }
            public bool foreign_parts_only { get; set; }
            public bool auto_translation { get; set; }
            public bool ai_translated { get; set; }
            public string machine_translated { get; set; }
            public DateTime upload_date { get; set; }
            public string release { get; set; }
            public string comments { get; set; }
            public int legacy_subtitle_id { get; set; }
            public Uploader uploader { get; set; }
            public FeatureDetails feature_details { get; set; }
            public string url { get; set; }

            public File[] files { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public class Uploader
        {
            public int uploader_id { get; set; }
            public string name { get; set; }
            public string rank { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public class FeatureDetails
        {
            public int feature_id { get; set; }

            public string feature_type { get; set; }
            public int year { get; set; }
            public string title { get; set; }
            public string movie_name { get; set; }
            public int imdb_id { get; set; }
            public int tmdb_id { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public class RelatedLinks
        {
            public string label { get; set; }
            public string url { get; set; }
            public string img_url { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public class File
        {
            public int file_id { get; set; }
            public int cd_number { get; set; }
            public string file_name { get; set; }

            public override string ToString()
            {
                return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
            }
        }

        public override string ToString()
        {
            return Util.JsonPrettify(Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }

#pragma warning restore IDE1006 // Naming Styles
    }
}