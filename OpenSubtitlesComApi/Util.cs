using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenSubtitlesComApi
{
    internal static class Util
    {
        public static string JsonPrettify(string json)
        {
            using System.IO.StringReader stringReader = new System.IO.StringReader(json);
            using System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            JsonTextReader jsonReader = new JsonTextReader(stringReader);
            JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }

        /**
         * Source: https://forum.opensubtitles.org/viewtopic.php?t=1562#p29506
         * Permalink: https://web.archive.org/web/20151215123021/http://forum.opensubtitles.org/viewtopic.php?t=1562#p29506
         * This code is NOT under the MIT license and used in the understanding that it was intended to be under a permissive license.
         **/

        public static string GetHash(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                throw new ArgumentException($"'{nameof(filepath)}' cannot be null or whitespace", nameof(filepath));
            }

            using FileStream input = File.OpenRead(filepath);
            ulong lhash = (ulong)input.Length;
            byte[] buf = new byte[65536 * 2];

            input.Read(buf, 0, 65536);
            input.Position = Math.Max(0, input.Length - 65536);
            input.Read(buf, 65536, 65536);

            for (int i = 0; i < 2 * 65536; i += 8) unchecked
                {
                    lhash += BitConverter.ToUInt64(buf, i);
                }

            return lhash.ToString("x16");
        }
    }
}