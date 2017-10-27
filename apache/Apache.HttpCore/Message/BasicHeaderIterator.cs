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

using System;
using Org.Apache.Http;
using Org.Apache.Http.Util;
using Sharpen;

namespace Org.Apache.Http.Message
{
	/// <summary>
	/// Basic implementation of a
	/// <see cref="Org.Apache.Http.HeaderIterator">Org.Apache.Http.HeaderIterator</see>
	/// .
	/// </summary>
	/// <since>4.0</since>
	public class BasicHeaderIterator : HeaderIterator
	{
		/// <summary>An array of headers to iterate over.</summary>
		/// <remarks>
		/// An array of headers to iterate over.
		/// Not all elements of this array are necessarily part of the iteration.
		/// This array will never be modified by the iterator.
		/// Derived implementations are expected to adhere to this restriction.
		/// </remarks>
		protected internal readonly Header[] allHeaders;

		/// <summary>
		/// The position of the next header in
		/// <see cref="allHeaders">allHeaders</see>
		/// .
		/// Negative if the iteration is over.
		/// </summary>
		protected internal int currentIndex;

		/// <summary>The header name to filter by.</summary>
		/// <remarks>
		/// The header name to filter by.
		/// <code>null</code> to iterate over all headers in the array.
		/// </remarks>
		protected internal string headerName;

		/// <summary>Creates a new header iterator.</summary>
		/// <remarks>Creates a new header iterator.</remarks>
		/// <param name="headers">an array of headers over which to iterate</param>
		/// <param name="name">
		/// the name of the headers over which to iterate, or
		/// <code>null</code> for any
		/// </param>
		public BasicHeaderIterator(Header[] headers, string name) : base()
		{
			this.allHeaders = Args.NotNull(headers, "Header array");
			this.headerName = name;
			this.currentIndex = FindNext(-1);
		}

		/// <summary>Determines the index of the next header.</summary>
		/// <remarks>Determines the index of the next header.</remarks>
		/// <param name="pos">
		/// one less than the index to consider first,
		/// -1 to search for the first header
		/// </param>
		/// <returns>
		/// the index of the next header that matches the filter name,
		/// or negative if there are no more headers
		/// </returns>
		protected internal virtual int FindNext(int pos)
		{
			int from = pos;
			if (from < -1)
			{
				return -1;
			}
			int to = this.allHeaders.Length - 1;
			bool found = false;
			while (!found && (from < to))
			{
				from++;
				found = FilterHeader(from);
			}
			return found ? from : -1;
		}

		/// <summary>Checks whether a header is part of the iteration.</summary>
		/// <remarks>Checks whether a header is part of the iteration.</remarks>
		/// <param name="index">the index of the header to check</param>
		/// <returns>
		/// <code>true</code> if the header should be part of the
		/// iteration, <code>false</code> to skip
		/// </returns>
		protected internal virtual bool FilterHeader(int index)
		{
			return (this.headerName == null) || Sharpen.Runtime.EqualsIgnoreCase(this.headerName
				, this.allHeaders[index].GetName());
		}

		// non-javadoc, see interface HeaderIterator
		public virtual bool HasNext()
		{
			return (this.currentIndex >= 0);
		}

		/// <summary>Obtains the next header from this iteration.</summary>
		/// <remarks>Obtains the next header from this iteration.</remarks>
		/// <returns>the next header in this iteration</returns>
		/// <exception cref="Sharpen.NoSuchElementException">if there are no more headers</exception>
		public virtual Header NextHeader()
		{
			int current = this.currentIndex;
			if (current < 0)
			{
				throw new NoSuchElementException("Iteration already finished.");
			}
			this.currentIndex = FindNext(current);
			return this.allHeaders[current];
		}

		/// <summary>Returns the next header.</summary>
		/// <remarks>
		/// Returns the next header.
		/// Same as
		/// <see cref="NextHeader()">nextHeader</see>
		/// , but not type-safe.
		/// </remarks>
		/// <returns>the next header in this iteration</returns>
		/// <exception cref="Sharpen.NoSuchElementException">if there are no more headers</exception>
		public object Next()
		{
			return NextHeader();
		}

		/// <summary>Removing headers is not supported.</summary>
		/// <remarks>Removing headers is not supported.</remarks>
		/// <exception cref="System.NotSupportedException">always</exception>
		public virtual void Remove()
		{
			throw new NotSupportedException("Removing headers is not supported.");
		}
	}
}
