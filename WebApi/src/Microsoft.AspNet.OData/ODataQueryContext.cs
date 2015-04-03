using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Core.UriParser.Semantic;
using Microsoft.OData.Edm;
using Microsoft.AspNet.OData.Common;
using Microsoft.AspNet.OData.Extensions;

namespace Microsoft.AspNet.OData
{
    /// <summary>
    /// This defines some context information used to perform query composition. 
    /// </summary>
    public class ODataQueryContext
    {
        /// <summary>
        /// Constructs an instance of <see cref="ODataQueryContext"/> with <see cref="IEdmModel" />, element CLR type,
        /// and <see cref="ODataPath" />.
        /// </summary>
        /// <param name="model">The EdmModel that includes the <see cref="IEdmType"/> corresponding to
        /// the given <paramref name="elementClrType"/>.</param>
        /// <param name="elementClrType">The CLR type of the element of the collection being queried.</param>
        /// <param name="path">The parsed <see cref="ODataPath"/>.</param>
        public ODataQueryContext(IEdmModel model, Type elementClrType, ODataPath path)
        {
            if (model == null)
            {
                throw Error.ArgumentNull("model");
            }

            if (elementClrType == null)
            {
                throw Error.ArgumentNull("elementClrType");
            }

            ElementType = model.GetEdmType(elementClrType);
            if (ElementType == null)
            {
                throw Error.Argument("elementClrType", "No EdmType found for the given ClrType");
            }

            ElementClrType = elementClrType;
            Model = model;
            Path = path;
            NavigationSource = GetNavigationSource(Model, ElementType);
        }

        /// <summary>
        /// Gets the given <see cref="IEdmModel"/> that contains the EntitySet.
        /// </summary>
        public IEdmModel Model { get; }

        /// <summary>
        /// Gets the <see cref="IEdmType"/> of the element.
        /// </summary>
        public IEdmType ElementType { get; }

        /// <summary>
        /// Gets the <see cref="IEdmNavigationSource"/> that contains the element.
        /// </summary>
        public IEdmNavigationSource NavigationSource { get; }

        /// <summary>
        /// Gets the CLR type of the element.
        /// </summary>
        public Type ElementClrType { get; }

        /// <summary>
        /// Gets the <see cref="ODataPath"/>.
        /// </summary>
        public ODataPath Path { get; private set; }

        private static IEdmNavigationSource GetNavigationSource(IEdmModel model, IEdmType elementType)
        {
            return model.EntityContainer?.EntitySets().FirstOrDefault(e => e.EntityType() == elementType);
        }
    }
}