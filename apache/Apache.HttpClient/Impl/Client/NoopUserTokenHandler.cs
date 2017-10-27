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

using Apache.Http.Client;
using Apache.Http.Impl.Client;
using Apache.Http.Protocol;
using Sharpen;

namespace Apache.Http.Impl.Client
{
	/// <summary>
	/// Noop implementation of
	/// <see cref="Apache.Http.Client.UserTokenHandler">Apache.Http.Client.UserTokenHandler
	/// 	</see>
	/// that always returns <code>null</code>.
	/// </summary>
	/// <since>4.3</since>
	public class NoopUserTokenHandler : UserTokenHandler
	{
		public static readonly NoopUserTokenHandler Instance = new NoopUserTokenHandler();

		public virtual object GetUserToken(HttpContext context)
		{
			return null;
		}
	}
}
