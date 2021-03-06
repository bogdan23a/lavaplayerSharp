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

using Apache.Http.Auth;
using Apache.Http.Impl.Auth;
using Apache.Http.Params;
using Apache.Http.Protocol;
using Sharpen;

namespace Apache.Http.Impl.Auth
{
	/// <summary>
	/// <see cref="Apache.Http.Auth.AuthSchemeProvider">Apache.Http.Auth.AuthSchemeProvider
	/// 	</see>
	/// implementation that creates and initializes
	/// <see cref="SPNegoScheme">SPNegoScheme</see>
	/// instances.
	/// </summary>
	/// <since>4.2</since>
	public class SPNegoSchemeFactory : AuthSchemeFactory, AuthSchemeProvider
	{
		private readonly bool stripPort;

		public SPNegoSchemeFactory(bool stripPort) : base()
		{
			this.stripPort = stripPort;
		}

		public SPNegoSchemeFactory() : this(false)
		{
		}

		public virtual bool IsStripPort()
		{
			return stripPort;
		}

		public virtual AuthScheme NewInstance(HttpParams @params)
		{
			return new SPNegoScheme(this.stripPort);
		}

		public virtual AuthScheme Create(HttpContext context)
		{
			return new SPNegoScheme(this.stripPort);
		}
	}
}
