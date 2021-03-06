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
using Apache.Http.Config;
using Apache.Http.IO;
using Apache.Http.Impl;
using Apache.Http.Impl.Conn;
using Apache.Http.Message;
using Sharpen;

namespace Apache.Http.Impl.Conn
{
	/// <summary>Default factory for response message parsers.</summary>
	/// <remarks>Default factory for response message parsers.</remarks>
	/// <since>4.3</since>
	public class DefaultHttpResponseParserFactory : HttpMessageParserFactory<HttpResponse
		>
	{
		public static readonly Apache.Http.Impl.Conn.DefaultHttpResponseParserFactory Instance
			 = new Apache.Http.Impl.Conn.DefaultHttpResponseParserFactory();

		private readonly LineParser lineParser;

		private readonly HttpResponseFactory responseFactory;

		public DefaultHttpResponseParserFactory(LineParser lineParser, HttpResponseFactory
			 responseFactory) : base()
		{
			this.lineParser = lineParser != null ? lineParser : BasicLineParser.Instance;
			this.responseFactory = responseFactory != null ? responseFactory : DefaultHttpResponseFactory
				.Instance;
		}

		public DefaultHttpResponseParserFactory(HttpResponseFactory responseFactory) : this
			(null, responseFactory)
		{
		}

		public DefaultHttpResponseParserFactory() : this(null, null)
		{
		}

		public virtual HttpMessageParser<HttpResponse> Create(SessionInputBuffer buffer, 
			MessageConstraints constraints)
		{
			return new DefaultHttpResponseParser(buffer, lineParser, responseFactory, constraints
				);
		}
	}
}
