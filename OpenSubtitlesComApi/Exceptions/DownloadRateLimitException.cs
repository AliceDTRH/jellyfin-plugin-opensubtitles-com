using System;
using System.Runtime.Serialization;

namespace OpenSubtitlesComApi.Exceptions
{
    [Serializable]
    public class DownloadRateLimitException : Exception
    {
        public DownloadRateLimitException()
        {
        }

        public DownloadRateLimitException(string message) : base(message)
        {
        }

        public DownloadRateLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DownloadRateLimitException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}