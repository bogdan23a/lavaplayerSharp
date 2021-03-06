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
using Org.Apache.Http.IO;
using Org.Apache.Http.Impl.IO;
using Org.Apache.Http.Message;
using Sharpen;

namespace Org.Apache.Http.Impl.IO
{
	/// <summary>
	/// HTTP response writer that serializes its output to an instance of
	/// <see cref="Org.Apache.Http.IO.SessionOutputBuffer">Org.Apache.Http.IO.SessionOutputBuffer
	/// 	</see>
	/// .
	/// </summary>
	/// <since>4.3</since>
	public class DefaultHttpResponseWriter : AbstractMessageWriter<HttpResponse>
	{
		/// <summary>Creates an instance of DefaultHttpResponseWriter.</summary>
		/// <remarks>Creates an instance of DefaultHttpResponseWriter.</remarks>
		/// <param name="buffer">the session output buffer.</param>
		/// <param name="formatter">
		/// the line formatter If <code>null</code>
		/// <see cref="Org.Apache.Http.Message.BasicLineFormatter.Instance">Org.Apache.Http.Message.BasicLineFormatter.Instance
		/// 	</see>
		/// will be used.
		/// </param>
		public DefaultHttpResponseWriter(SessionOutputBuffer buffer, LineFormatter formatter
			) : base(buffer, formatter)
		{
		}

		public DefaultHttpResponseWriter(SessionOutputBuffer buffer) : base(buffer, null)
		{
		}

		/// <exception cref="System.IO.IOException"></exception>
		protected internal override void WriteHeadLine(HttpResponse message)
		{
			lineFormatter.FormatStatusLine(this.lineBuf, message.GetStatusLine());
			this.sessionBuffer.WriteLine(this.lineBuf);
		}
	}
}
