using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Common;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Library;

namespace Microsoft.AspNet.OData
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class EnableQueryAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context == null)
            {
                throw Error.ArgumentNull("context");
            }

            var response = context.HttpContext.Response;
            if (!response.HasSuccessfulStatusCode())
            {
                return;
            }

            var result = context.Result as ObjectResult;
            if (result == null)
            {
                throw Error.Argument("context", "ActionExecutedContext should contain ObjectResult");
            }

            var request = context.HttpContext.Request;
            var shouldApplyQueryOption = result.Value != null && request.HasQueryOption();
            if (shouldApplyQueryOption)
            {
                result.Value = ApplyQueryOption(result.Value, request, context.ActionDescriptor);
            }
        }

        private object ApplyQueryOption(object value, HttpRequest request, ActionDescriptor descriptor)
        {
            var elementClrType = TypeHelper.GetImplementedIEnumerableType(value.GetType());

            var model = request.ODataProperties().Model;
            if (model == null)
            {
                throw Error.InvalidOperation("Model must be provided");
            }

            var queryContext = new ODataQueryContext(
                model,
                elementClrType,
                request.ODataProperties().Path);

            var queryOptions = new ODataQueryOptions(queryContext, request);

            var enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                // response is single entity.
                return value;
            }

            // response is a collection.
            var query = (value as IQueryable) ?? enumerable.AsQueryable();
            return queryOptions.ApplyTo(query,
                new ODataQuerySettings
                {
                    HandleNullPropagation = HandleNullPropagationOption.True
                });
        }
    }
}