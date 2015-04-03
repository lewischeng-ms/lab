using System.Collections.Generic;

namespace SampleApi.Models
{
    public class InMemoryResource<T> : List<T> where T : class
    {

    }
}