using System;
using System.Collections.Generic;

namespace SampleApi.Models
{
    public class SampleContext
    {
        private readonly InMemoryResource<Product> _products = new InMemoryResource<Product>
        {
            new Product { Id = 1, Name = "Apple",  Price = 10 },
            new Product { Id = 2, Name = "Orange", Price = 20 },
            new Product { Id = 3, Name = "Banana", Price = 30 }
        };

        private readonly InMemoryResource<Customer> _customers = new InMemoryResource<Customer>
        {
            new Customer { Id = 1, Name = "Lewis" }
        };
        
        public SampleContext()
        {
            _customers[0].Products = new List<Product> { _products[0], _products[1] };
        }

        public InMemoryResource<Product> Products => _products;

        public InMemoryResource<Customer> Customers => _customers;

        private static SampleContext _instance;

        public static SampleContext Instance()
        {
            if (_instance == null)
            {
                _instance = new SampleContext();
            }

            return _instance;
        }
    }
}