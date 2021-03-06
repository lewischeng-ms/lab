﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.OData.Common;
using Microsoft.AspNet.Mvc;

namespace Microsoft.AspNet.OData
{
    internal static class TypeHelper
    {
        /// <summary>
        /// Returns type of T if the type implements IEnumerable of T, otherwise, return null.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static Type GetImplementedIEnumerableType(Type type)
        {
            if (type == null)
            {
                throw Error.ArgumentNull("type");
            }
            
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                {
                    return interfaceType.GenericTypeArguments.First();
                }
            }

            return null;
        }

        internal static IEnumerable<Type> GetLoadedTypes(IAssemblyProvider assemblyProvider)
        {
            List<Type> result = new List<Type>();

            // Go through all assemblies referenced by the application and search for types matching a predicate
            IEnumerable<Assembly> assemblies = assemblyProvider.CandidateAssemblies;
            foreach (Assembly assembly in assemblies)
            {
                Type[] exportedTypes = null;
                if (assembly == null || assembly.IsDynamic)
                {
                    // can't call GetTypes on a null (or dynamic?) assembly
                    continue;
                }

                try
                {
                    exportedTypes = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    exportedTypes = ex.Types;
                }
                catch
                {
                    continue;
                }

                if (exportedTypes != null)
                {
                    result.AddRange(exportedTypes.Where(t => t != null && t.GetTypeInfo().IsVisible));
                }
            }

            return result;
        }

        public static Type GetUnderlyingTypeOrSelf(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool IsEnum(Type type)
        {
            Type underlyingTypeOrSelf = GetUnderlyingTypeOrSelf(type);
            return underlyingTypeOrSelf.GetTypeInfo().IsEnum;
        }
    }
}