using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.OData.Common;

namespace Microsoft.AspNet.OData.Query
{
    public class ModelAssemblyProvider : IAssemblyProvider
    {
        private readonly Assembly _modelAssembly;

        public ModelAssemblyProvider(Type modelType)
        {
            if (modelType == null)
            {
                throw Error.ArgumentNull("modelType");
            }

            _modelAssembly = modelType.GetTypeInfo().Assembly;
        }

        public IEnumerable<Assembly> CandidateAssemblies
        {
            get
            {
                yield return _modelAssembly;
            }
        }
    }
}