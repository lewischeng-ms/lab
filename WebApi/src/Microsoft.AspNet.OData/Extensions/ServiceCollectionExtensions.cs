using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData.Formatters;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.OData.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddORest(this IServiceCollection services)
        {
            return services.Configure<MvcOptions>(options =>
            {
                options.OutputFormatters.Insert(0, new ODataOutputFormatter());
            });
        }
    }
}