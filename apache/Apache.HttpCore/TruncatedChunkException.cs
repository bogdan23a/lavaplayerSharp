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
using Sharpen;

namespace Org.Apache.Http
{
	/// <summary>Signals a truncated chunk in a chunked stream.</summary>
	/// <remarks>Signals a truncated chunk in a chunked stream.</remarks>
	/// <since>4.1</since>
	[System.Serializable]
	public class TruncatedChunkException : MalformedChunkCodingException
	{
		private const long serialVersionUID = -23506263930279460L;

		/// <summary>Creates a TruncatedChunkException with the specified detail message.</summary>
		/// <remarks>Creates a TruncatedChunkException with the specified detail message.</remarks>
		/// <param name="message">The exception detail message</param>
		public TruncatedChunkException(string message) : base(message)
		{
		}
	}
}
