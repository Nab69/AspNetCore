// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    internal class DynamicControllerEndpointSelector : IDisposable
    {
        private readonly ControllerActionEndpointDataSource _dataSource;
        private readonly DataSourceDependentCache<ActionSelectionTable<RouteEndpoint>> _cache;

        public DynamicControllerEndpointSelector(ControllerActionEndpointDataSource dataSource)
        {
            if (dataSource == null)
            {
                throw new ArgumentNullException(nameof(dataSource));
            }

            _dataSource = dataSource;
            _cache = new DataSourceDependentCache<ActionSelectionTable<RouteEndpoint>>(dataSource, Initialize);
        }

        private ActionSelectionTable<RouteEndpoint> CurrentState => _cache.EnsureInitialized();

        // This is async because the page version will be async. We don't want to put ourselves in a
        // position where these types become public and then users start using them in sync locations.
        public Task<IReadOnlyList<RouteEndpoint>> SelectEndpointsAsync(RouteValueDictionary values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var state = CurrentState;

            // The ActionSelectionTable works based on a string[] of the route values in a pre-calculated order. This code extracts
            // those values in the correct order.
            var keys = state.RouteKeys;
            var extractedValues = new string[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                values.TryGetValue(keys[i], out var obj);
                extractedValues[i] = Convert.ToString(obj, CultureInfo.InvariantCulture) ?? string.Empty;
            }

            if (state.OrdinalEntries.TryGetValue(extractedValues, out var matches) ||
                state.OrdinalIgnoreCaseEntries.TryGetValue(extractedValues, out matches))
            {
                return Task.FromResult<IReadOnlyList<RouteEndpoint>>(matches);
            }

            return Task.FromResult<IReadOnlyList<RouteEndpoint>>(Array.Empty<RouteEndpoint>());
        }

        private static ActionSelectionTable<RouteEndpoint> Initialize(IReadOnlyList<Endpoint> endpoints)
        {
            return ActionSelectionTable<RouteEndpoint>.Create(endpoints);
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}
