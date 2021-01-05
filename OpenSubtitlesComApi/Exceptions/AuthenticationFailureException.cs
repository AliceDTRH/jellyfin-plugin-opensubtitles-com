using System;
using System.Runtime.Serialization;

namespace OpenSubtitlesComApi
{
    [Serializable]
    public class AuthenticationFailureException : Exception
    {
        public AuthenticationFailureException()
        {
        }

        public AuthenticationFailureException(string message) : base(message)
        {
        }

        public AuthenticationFailureException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AuthenticationFailureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}