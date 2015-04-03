﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using Microsoft.AspNet.OData.Common;
using System.Web.OData.Query;

namespace System.Web.OData.Builder.Conventions.Attributes
{
    internal class NotExpandableAttributeEdmPropertyConvention : AttributeEdmPropertyConvention<NavigationPropertyConfiguration>
    {
        public NotExpandableAttributeEdmPropertyConvention()
            : base(attribute => attribute.GetType() == typeof(NotExpandableAttribute), allowMultiple: false)
        {
        }

        public override void Apply(NavigationPropertyConfiguration edmProperty,
            StructuralTypeConfiguration structuralTypeConfiguration,
            Attribute attribute,
            ODataConventionModelBuilder model)
        {
            if (edmProperty == null)
            {
                throw Error.ArgumentNull("edmProperty");
            }

            if (!edmProperty.AddedExplicitly)
            {
                edmProperty.IsNotExpandable();
            }
        }
    }
}
