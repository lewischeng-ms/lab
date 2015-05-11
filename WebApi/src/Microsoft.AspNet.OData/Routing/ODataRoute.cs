using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Web.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.OptionsModel;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace Microsoft.AspNet.OData.Routing
{
    public class ODataRoute : IRouter
    {
        private readonly IODataRoutingConvention _routingConvention;

        public ODataRoute(string routePrefix, Type apiContextType)
        {
            RoutePrefix = routePrefix;
            ApiContextType = apiContextType;
            _routingConvention = new DefaultODataRoutingConvention();
        }

        public string RoutePrefix { get; }

        public Type ApiContextType { get; }

        public async Task RouteAsync(RouteContext context)
        {
            var request = context.HttpContext.Request;
            var model = BuildEdmModel();
            request.ODataProperties().Model = model;
            request.ODataProperties().ApiContextType = ApiContextType;
            
            var parser = new ODataUriParser(model, request.GetPathUri(RoutePrefix));
            var path = parser.ParsePath();

            var actionDescriptor = _routingConvention.SelectControllerAction(path, context);
            await InvokeActionAsync(context, actionDescriptor);
            context.IsHandled = true;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            throw new NotImplementedException();
        }

        private IEdmModel BuildEdmModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = ApiContextType.Namespace;

            var publicProperties = ApiContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in publicProperties)
            {
                var entityClrType = TypeHelper.GetImplementedIEnumerableType(property.PropertyType);
                EntityTypeConfiguration entity = builder.AddEntityType(entityClrType);
                builder.AddEntitySet(property.Name, entity);
            }
            
            return builder.GetEdmModel();
        }

        private async Task InvokeActionAsync(RouteContext context, ActionDescriptor actionDescriptor)
        {
            var services = context.HttpContext.RequestServices;
            Debug.Assert(services != null);

            var actionContext = new ActionContext(context.HttpContext, context.RouteData, actionDescriptor);

            var optionsAccessor = services.GetRequiredService<IOptions<MvcOptions>>();
            actionContext.ModelState.MaxAllowedErrors = optionsAccessor.Options.MaxModelValidationErrors;

            var contextAccessor = services.GetRequiredService<IScopedInstance<ActionContext>>();
            contextAccessor.Value = actionContext;
            var invokerFactory = services.GetRequiredService<IActionInvokerFactory>();
            var invoker = invokerFactory.CreateInvoker(actionContext);

            await invoker.InvokeAsync();
        }
    }
}