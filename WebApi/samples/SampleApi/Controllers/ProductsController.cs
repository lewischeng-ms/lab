using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData;
using SampleApi.Models;

namespace SampleApi.Controllers
{
    [EnableQuery]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return SampleContext.Instance().Products;
        }

        // GET api/Products/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return SampleContext.Instance().Products.Single(p => p.Id == id);
        }

        // POST api/Products
        [HttpPost]
        public void Post([FromBody]Product value)
        {
        }

        // PUT api/Products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Product value)
        {
        }

        // DELETE api/Products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
