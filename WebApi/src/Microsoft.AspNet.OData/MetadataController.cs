using System;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.OData.Edm;

namespace Microsoft.AspNet.OData
{
    [Route("api/[controller]")]
    public class MetadataController : Controller
    {
        // GET: api/$metadata
        [HttpGet]
        public IEdmModel Get()
        {
            return Request.ODataProperties().Model;
        }
    }
}