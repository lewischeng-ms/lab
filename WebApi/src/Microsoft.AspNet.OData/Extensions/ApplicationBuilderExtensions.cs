using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.AspNet.Mvc.Routing;
using Microsoft.AspNet.OData.Formatters;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseORest(this IApplicationBuilder app, string routePrefix, Type apiContextType)
        {
            return app.UseRouter(new ODataRoute(routePrefix, apiContextType));
        }
    }
}