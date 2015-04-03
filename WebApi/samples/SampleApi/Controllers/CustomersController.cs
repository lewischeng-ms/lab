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
    public class CustomersController : Controller
    {
        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return SampleContext.Instance().Customers;
        }

        // GET api/Customers/5
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            return SampleContext.Instance().Customers.Single(c => c.Id == id);
        }

        // GET api/Customers/5/Products
        [HttpGet("{id}/Products")]
        public IEnumerable<Product> GetProducts(int id)
        {
            return Get(id).Products;
        }

        // POST api/Customers
        [HttpPost]
        public void Post([FromBody]Customer value)
        {
        }

        // PUT api/Customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Customer value)
        {
        }

        // DELETE api/Customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
