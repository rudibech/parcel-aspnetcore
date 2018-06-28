using System;

namespace RudiBech.AspNetCore.SpaServices.Parcel
{
    /// <summary>
    /// Copy of https://github.com/aspnet/JavaScriptServices/blob/dev/src/Microsoft.AspNetCore.SpaServices/Webpack/ConditionalProxyMiddlewareOptions.cs
    /// </summary>
    public class ConditionalProxyMiddlewareOptions
    {
        public ConditionalProxyMiddlewareOptions(string scheme, string host, string port, TimeSpan requestTimeout)
        {
            Scheme = scheme;
            Host = host;
            Port = port;
            RequestTimeout = requestTimeout;
        }

        public string Scheme { get; }
        public string Host { get; }
        public string Port { get; }
        public TimeSpan RequestTimeout { get; }
    }

}