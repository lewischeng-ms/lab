﻿using Microsoft.AspNet.Http;
using System;

namespace Microsoft.AspNet.OData.Extensions
{
    public static class HttpResponseExtensions
    {
        public static bool HasSuccessfulStatusCode(this HttpResponse response)
        {
            return response?.StatusCode >= 200 && response.StatusCode < 300;
        }
    }
}