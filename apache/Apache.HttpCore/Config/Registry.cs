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

using System.Collections.Generic;
using System.Globalization;
using Sharpen;

namespace Org.Apache.Http.Config
{
	/// <summary>Generic registry of items keyed by low-case string ID.</summary>
	/// <remarks>Generic registry of items keyed by low-case string ID.</remarks>
	/// <since>4.3</since>
	public sealed class Registry<I> : Org.Apache.Http.Config.Lookup<I>
	{
		private readonly IDictionary<string, I> map;

		internal Registry(IDictionary<string, I> map) : base()
		{
            this.map = new ConcurrentHashMap<string, I>(map);
		}

		public I Lookup(string key)
		{
			if (key == null)
			{
                return default(I);
			}
			return map.Get(key.ToLower(CultureInfo.InvariantCulture));
		}

		public override string ToString()
		{
			return map.ToString();
		}
	}
}
