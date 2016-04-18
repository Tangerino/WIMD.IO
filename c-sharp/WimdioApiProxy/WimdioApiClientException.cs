using System;

namespace WimdioApiProxy.v2
{
    public class WimdioApiClientException : ApplicationException
    {
        public WimdioApiClientException()
            : base() { }

        public WimdioApiClientException(string message)
            : base(message) { }

        public WimdioApiClientException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public WimdioApiClientException(string message, Exception innerException)
            : base(message, innerException) { }

        public WimdioApiClientException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }
    }
}
