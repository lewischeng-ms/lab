using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Extensions
{
    public class HttpRequestProperties
    {
        private const string ApiContextTypeKey = "Microsoft.AspNet.OData.ApiContextType";
        private const string ModelKey = "Microsoft.AspNet.OData.Model";
        private const string PathKey = "Microsoft.AspNet.OData.Path";

        private readonly IDictionary<string, object> _properties = new ConcurrentDictionary<string, object>();

        private HttpRequest _request;

        internal HttpRequestProperties(HttpRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets or sets the ApiContext type associated with the request.
        /// </summary>
        public Type ApiContextType
        {
            get
            {
                return GetValueOrNull<Type>(ApiContextTypeKey);
            }
            set
            {
                _properties[ApiContextTypeKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the EDM model associated with the request.
        /// </summary>
        public IEdmModel Model
        {
            get
            {
                return GetValueOrNull<IEdmModel>(ModelKey);
            }
            set
            {
                _properties[ModelKey] = value;
            }
        }

        /// <summary>
        /// Gets or sets the OData path of the request.
        /// </summary>
        public ODataPath Path
        {
            get
            {
                return GetValueOrNull<ODataPath>(PathKey);
            }
            set
            {
                _properties[PathKey] = value;
            }
        }

        private T GetValueOrNull<T>(string propertyName) where T : class
        {
            object value;
            if (_properties.TryGetValue(propertyName, out value))
            {
                // Fairly big problem if following cast fails. Indicates something else is writing properties with
                // names we've chosen. Do not silently return null because that will hide the problem.
                return (T)value;
            }

            return null;
        }
    }
}