// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Contains extension methods for using static files with endpoint routing.
    /// </summary>
    public static class StaticFilesEndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Registers a fallback <see cref="RouteEndpoint"/> that will serve the provided static file
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <param name="filePath">The file path of the file to serve.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/></returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> is intended to handle cases where URL path of
        /// the request does not contain a filename, and no other endpoint has matched. This is convenient for routing
        /// requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
        /// result in an HTTP 404.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> with use the <see cref="StaticFileMiddleware"/>
        /// to serve requests for the provided <paramref name="filePath"/>. The default <see cref="StaticFileOptions"/>
        /// will be used.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> registers an endpoint using the pattern
        /// <c>{*path:nonfile}</c>. The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallbackToFile(
            this IEndpointRouteBuilder builder,
            string filePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return builder.MapFallback(CreateRequestDelegate(builder, filePath));
        }

        /// <summary>
        /// Registers a fallback <see cref="RouteEndpoint"/> that will serve the provided static file
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <param name="filePath">The file path of the file to serve.</param>
        /// <param name="options"><see cref="StaticFileOptions"/> for the <see cref="StaticFileMiddleware"/>.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/></returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> is intended to handle cases where URL path of
        /// the request does not contain a filename, and no other endpoint has matched. This is convenient for routing
        /// requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
        /// result in an HTTP 404.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> with use the <see cref="StaticFileMiddleware"/>
        /// to serve requests for the provided <paramref name="filePath"/>.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> registers an endpoint using the pattern
        /// <c>{*path:nonfile}</c>. The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallbackToFile(
            this IEndpointRouteBuilder builder,
            string filePath,
            StaticFileOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return builder.MapFallback(CreateRequestDelegate(builder, filePath, options));
        }

        /// <summary>
        /// Registers a fallback <see cref="RouteEndpoint"/> that will serve the provided static file
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <param name="pattern">The route pattern.</param>
        /// <param name="filePath">The file path of the file to serve.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/></returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> is intended to handle cases where URL path of
        /// the request does not contain a filename, and no other endpoint has matched. This is convenient for routing
        /// requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
        /// result in an HTTP 404.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> with use the <see cref="StaticFileMiddleware"/>
        /// to serve requests for the provided <paramref name="filePath"/>. The default <see cref="StaticFileOptions" />
        /// will be used.
        /// </para>
        /// <para>
        /// The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// <para>
        /// This overload will use the provided <paramref name="pattern"/> verbatim. Use the <c>:nonfile</c> route contraint
        /// to exclude requests for static files.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallbackToFile(
            this IEndpointRouteBuilder builder,
            string pattern,
            string filePath)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return builder.MapFallback(pattern, CreateRequestDelegate(builder, filePath));
        }

        /// <summary>
        /// Registers a fallback <see cref="RouteEndpoint"/> that will serve the provided static file
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointRouteBuilder"/>.</param>\
        /// <param name="pattern">The route pattern.</param>
        /// <param name="filePath">The file path of the file to serve.</param>
        /// <param name="options"><see cref="StaticFileOptions"/> for the <see cref="StaticFileMiddleware"/>.</param>
        /// <returns>The <see cref="IEndpointRouteBuilder"/></returns>
        /// <remarks>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> is intended to handle cases where URL path of
        /// the request does not contain a filename, and no other endpoint has matched. This is convenient for routing
        /// requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
        /// result in an HTTP 404.
        /// </para>
        /// <para>
        /// <see cref="MapFallbackToFile(IEndpointRouteBuilder, string)"/> with use the <see cref="StaticFileMiddleware"/>
        /// to serve requests for the provided <paramref name="filePath"/>.
        /// </para>
        /// <para>
        /// The order of the registered endpoint will be <c>int.MaxValue</c>.
        /// </para>
        /// <para>
        /// This overload will use the provided <paramref name="pattern"/> verbatim. Use the <c>:nonfile</c> route contraint
        /// to exclude requests for static files.
        /// </para>
        /// </remarks>
        public static IEndpointConventionBuilder MapFallbackToFile(
            this IEndpointRouteBuilder builder,
            string pattern,
            string filePath,
            StaticFileOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return builder.MapFallback(pattern, CreateRequestDelegate(builder, filePath, options));
        }

        private static RequestDelegate CreateRequestDelegate(
            IEndpointRouteBuilder builder,
            string filePath,
            StaticFileOptions options = null)
        {
            var app = builder.CreateApplicationBuilder();
            app.Use(next => context =>
            {
                context.Request.Path = "/" + filePath;

                return next(context);
            });

            if (options == null)
            {
                app.UseStaticFiles();
            }
            else
            {
                app.UseStaticFiles(options);
            }

            return app.Build();
        }
    }
}
