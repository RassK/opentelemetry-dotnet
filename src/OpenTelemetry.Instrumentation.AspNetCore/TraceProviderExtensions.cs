// <copyright file="TraceProviderExtensions.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Instrumentation.AspNetCore.Implementation;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace
{
    /// <summary>
    /// Extension methods to simplify registering of ASP.NET Core request instrumentation.
    /// </summary>
    public static class TraceProviderExtensions
    {
        /// <summary>
        /// Enables the incoming requests automatic data collection for ASP.NET Core.
        /// </summary>
        /// <param name="provider"><see cref="TracerProvider"/> being configured.</param>
        /// <param name="configureAspNetCoreInstrumentationOptions">ASP.NET Core Request configuration options.</param>
        /// <returns>The instance of <see cref="TracerProvider"/> to chain the calls.</returns>
        public static TracerProvider AddAspNetCoreInstrumentation(
            this TracerProvider provider,
            Action<AspNetCoreInstrumentationOptions> configureAspNetCoreInstrumentationOptions = null)
        {
            Guard.ThrowIfNull(provider);

            return AddAspNetCoreInstrumentation(provider, new AspNetCoreInstrumentationOptions(), configureAspNetCoreInstrumentationOptions);
        }

        internal static TracerProvider AddAspNetCoreInstrumentation(
            this TracerProvider provider,
            AspNetCoreInstrumentation instrumentation)
        {
            return provider.AddInstrumentation(() => instrumentation);
        }

        private static TracerProvider AddAspNetCoreInstrumentation(
            TracerProvider provider,
            AspNetCoreInstrumentationOptions options,
            Action<AspNetCoreInstrumentationOptions> configure = null)
        {
            configure?.Invoke(options);
            return AddAspNetCoreInstrumentation(
                provider,
                new AspNetCoreInstrumentation(new HttpInListener(options)));
        }
    }
}
