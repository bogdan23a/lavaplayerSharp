/**
 * Couchbase Lite for .NET
 *
 * Original iOS version by Jens Alfke
 * Android Port by Marty Schoch, Traun Leyden
 * C# Port by Zack Gramana
 *
 * Copyright (c) 2012, 2013 Couchbase, Inc. All rights reserved.
 * Portions (c) 2013 Xamarin, Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the
 * License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
 * either express or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

using Org.Apache.Http.Protocol;
using Sharpen;

namespace Org.Apache.Http.Protocol
{
	/// <summary>
	/// HttpRequestHandlerResolver can be used to resolve an instance of
	/// <see cref="HttpRequestHandler">HttpRequestHandler</see>
	/// matching a particular request URI. Usually the
	/// mapped request handler will be used to process the request with the
	/// specified request URI.
	/// </summary>
	/// <since>4.0</since>
	[System.ObsoleteAttribute(@"see HttpRequestHandlerMapper")]
	public interface HttpRequestHandlerResolver
	{
		/// <summary>Looks up a handler matching the given request URI.</summary>
		/// <remarks>Looks up a handler matching the given request URI.</remarks>
		/// <param name="requestURI">the request URI</param>
		/// <returns>
		/// HTTP request handler or <code>null</code> if no match
		/// is found.
		/// </returns>
		HttpRequestHandler Lookup(string requestURI);
	}
}
