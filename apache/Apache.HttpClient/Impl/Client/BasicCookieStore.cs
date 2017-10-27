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
using System.Collections.Generic;
using Apache.Http.Client;
using Apache.Http.Cookie;
using Sharpen;

namespace Apache.Http.Impl.Client
{
	/// <summary>
	/// Default implementation of
	/// <see cref="Apache.Http.Client.CookieStore">Apache.Http.Client.CookieStore</see>
	/// </summary>
	/// <since>4.0</since>
	[System.Serializable]
	public class BasicCookieStore : CookieStore
	{
		private const long serialVersionUID = -7581093305228232025L;

		private readonly TreeSet<Apache.Http.Cookie.Cookie> cookies;

		public BasicCookieStore() : base()
		{
			this.cookies = new TreeSet<Apache.Http.Cookie.Cookie>(new CookieIdentityComparator
				());
		}

		/// <summary>
		/// Adds an
		/// <see cref="Apache.Http.Cookie.Cookie">HTTP cookie</see>
		/// , replacing any existing equivalent cookies.
		/// If the given cookie has already expired it will not be added, but existing
		/// values will still be removed.
		/// </summary>
		/// <param name="cookie">
		/// the
		/// <see cref="Apache.Http.Cookie.Cookie">cookie</see>
		/// to be added
		/// </param>
		/// <seealso cref="AddCookies(Apache.Http.Cookie.Cookie[])">AddCookies(Apache.Http.Cookie.Cookie[])
		/// 	</seealso>
		public virtual void AddCookie(Apache.Http.Cookie.Cookie cookie)
		{
			lock (this)
			{
				if (cookie != null)
				{
					// first remove any old cookie that is equivalent
					cookies.Remove(cookie);
					if (!cookie.IsExpired(new DateTime()))
					{
						cookies.AddItem(cookie);
					}
				}
			}
		}

		/// <summary>
		/// Adds an array of
		/// <see cref="Apache.Http.Cookie.Cookie">HTTP cookies</see>
		/// . Cookies are added individually and
		/// in the given array order. If any of the given cookies has already expired it will
		/// not be added, but existing values will still be removed.
		/// </summary>
		/// <param name="cookies">
		/// the
		/// <see cref="Apache.Http.Cookie.Cookie">cookies</see>
		/// to be added
		/// </param>
		/// <seealso cref="AddCookie(Apache.Http.Cookie.Cookie)">AddCookie(Apache.Http.Cookie.Cookie)
		/// 	</seealso>
		public virtual void AddCookies(Apache.Http.Cookie.Cookie[] cookies)
		{
			lock (this)
			{
				if (cookies != null)
				{
					foreach (Apache.Http.Cookie.Cookie cooky in cookies)
					{
						this.AddCookie(cooky);
					}
				}
			}
		}

		/// <summary>
		/// Returns an immutable array of
		/// <see cref="Apache.Http.Cookie.Cookie">cookies</see>
		/// that this HTTP
		/// state currently contains.
		/// </summary>
		/// <returns>
		/// an array of
		/// <see cref="Apache.Http.Cookie.Cookie">cookies</see>
		/// .
		/// </returns>
		public virtual IList<Apache.Http.Cookie.Cookie> GetCookies()
		{
			lock (this)
			{
				//create defensive copy so it won't be concurrently modified
				return new AList<Apache.Http.Cookie.Cookie>(cookies);
			}
		}

		/// <summary>
		/// Removes all of
		/// <see cref="Apache.Http.Cookie.Cookie">cookies</see>
		/// in this HTTP state
		/// that have expired by the specified
		/// <see cref="System.DateTime">date</see>
		/// .
		/// </summary>
		/// <returns>true if any cookies were purged.</returns>
		/// <seealso cref="Apache.Http.Cookie.Cookie.IsExpired(System.DateTime)">Apache.Http.Cookie.Cookie.IsExpired(System.DateTime)
		/// 	</seealso>
		public virtual bool ClearExpired(DateTime date)
		{
			lock (this)
			{
				if (date == null)
				{
					return false;
				}
				bool removed = false;
				for (IEnumerator<Apache.Http.Cookie.Cookie> it = cookies.GetEnumerator(); it.HasNext
					(); )
				{
					if (it.Next().IsExpired(date))
					{
						it.Remove();
						removed = true;
					}
				}
				return removed;
			}
		}

		/// <summary>Clears all cookies.</summary>
		/// <remarks>Clears all cookies.</remarks>
		public virtual void Clear()
		{
			lock (this)
			{
				cookies.Clear();
			}
		}

		public override string ToString()
		{
			lock (this)
			{
				return cookies.ToString();
			}
		}
	}
}
