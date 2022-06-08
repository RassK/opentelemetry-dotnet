// <copyright file="TracerProvider.cs" company="OpenTelemetry Authors">
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
using System.Collections.Concurrent;
using System.Diagnostics;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Trace
{
    /// <summary>
    /// TracerProvider is the entry point of the OpenTelemetry API. It provides access to <see cref="Tracer"/>.
    /// </summary>
    public class TracerProvider : BaseProvider
    {
        private readonly ConcurrentBag<object> instrumentations = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TracerProvider"/> class.
        /// </summary>
        protected TracerProvider()
        {
        }

        /// <summary>
        /// Gets the default Tracer.
        /// </summary>
        public static TracerProvider Default { get; } = new TracerProvider();

        /// <summary>
        /// Gets the registered instrumentation instances.
        /// </summary>
        internal ConcurrentBag<object> Instrumentations => this.instrumentations;

        /// <summary>
        /// Gets a tracer with given name and version.
        /// </summary>
        /// <param name="name">Name identifying the instrumentation library.</param>
        /// <param name="version">Version of the instrumentation library.</param>
        /// <returns>Tracer instance.</returns>
        public Tracer GetTracer(string name, string version = null)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            return new Tracer(new ActivitySource(name, version));
        }

        /// <summary>
        /// Tracks the instrumentation instance and disposes if is <see cref="IDisposable">IDisposable</see>.
        /// </summary>
        /// <param name="instrumentation">Instrumentation instance.</param>
        /// <returns>TracerProvider instance.</returns>
        public TracerProvider AddInstrumentation(object instrumentation)
        {
            Guard.ThrowIfNull(instrumentation);

            this.instrumentations.Add(instrumentation);

            return this;
        }
    }
}
