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

using Org.Apache.Http;
using Org.Apache.Http.Protocol;
using Org.Apache.Http.Util;
using Sharpen;

namespace Org.Apache.Http.Protocol
{
	/// <summary>
	/// ResponseDate is responsible for adding <code>Date<c/ode> header to the
	/// outgoing responses.
	/// </summary>
	/// <remarks>
	/// ResponseDate is responsible for adding <code>Date<c/ode> header to the
	/// outgoing responses. This interceptor is recommended for server side protocol
	/// processors.
	/// </remarks>
	/// <since>4.0</since>
	public class ResponseDate : HttpResponseInterceptor
	{
		private static readonly HttpDateGenerator DateGenerator = new HttpDateGenerator();

		public ResponseDate() : base()
		{
		}

		/// <exception cref="Org.Apache.Http.HttpException"></exception>
		/// <exception cref="System.IO.IOException"></exception>
		public virtual void Process(HttpResponse response, HttpContext context)
		{
			Args.NotNull(response, "HTTP response");
			int status = response.GetStatusLine().GetStatusCode();
			if ((status >= HttpStatus.ScOk) && !response.ContainsHeader(HTTP.DateHeader))
			{
				string httpdate = DateGenerator.GetCurrentDate();
				response.SetHeader(HTTP.DateHeader, httpdate);
			}
		}
	}
}
