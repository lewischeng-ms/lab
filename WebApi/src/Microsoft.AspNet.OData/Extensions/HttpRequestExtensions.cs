using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Common;

namespace Microsoft.AspNet.OData.Extensions
{
    public static class HttpRequestExtensions
    {
        private static readonly IDictionary<HttpRequest, HttpRequestProperties> HttpRequestPropertiesDict =
            new Dictionary<HttpRequest, HttpRequestProperties>();

        public static bool HasQueryOption(this HttpRequest request)
        {
            return request?.Query != null && request.Query.Count > 0;
        }

        public static Uri GetPathUri(this HttpRequest request, string routePrefix)
        {
            var prefix = PathString.FromUriComponent("/" + routePrefix);

            PathString remaining;
            if (request.Path.StartsWithSegments(prefix, out remaining))
            {
                return new Uri(remaining.ToString(), UriKind.Relative);
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="HttpRequestProperties"/> instance containing OData methods and properties
        /// for given <see cref="HttpRequest"/>.
        /// </summary>
        /// <param name="request">The request of interest.</param>
        /// <returns>
        /// An object through which OData methods and properties for given <paramref name="request"/> are available.
        /// </returns>
        public static HttpRequestProperties ODataProperties(this HttpRequest request)
        {
            if (request == null)
            {
                throw Error.ArgumentNull("request");
            }

            HttpRequestProperties properties;
            if (!HttpRequestPropertiesDict.TryGetValue(request, out properties))
            {
                properties = new HttpRequestProperties(request);
                HttpRequestPropertiesDict[request] = properties;
            }

            return properties;
        }
    }
}