using System;
using System.Collections.Generic;

namespace SampleApi.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Product> Products { get; set; } 
    }
}