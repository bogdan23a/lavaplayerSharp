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

using Apache.Http;
using Sharpen;

namespace Apache.Http.Impl.Execchain
{
	/// <summary>Signals that the tunnel request was rejected by the proxy host.</summary>
	/// <remarks>Signals that the tunnel request was rejected by the proxy host.</remarks>
	/// <since>4.0</since>
	[System.Serializable]
	public class TunnelRefusedException : HttpException
	{
		private const long serialVersionUID = -8646722842745617323L;

		private readonly HttpResponse response;

		public TunnelRefusedException(string message, HttpResponse response) : base(message
			)
		{
			this.response = response;
		}

		public virtual HttpResponse GetResponse()
		{
			return this.response;
		}
	}
}
