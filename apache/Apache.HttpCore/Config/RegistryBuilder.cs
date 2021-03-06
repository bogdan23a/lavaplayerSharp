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
using Org.Apache.Http.Config;
using Org.Apache.Http.Util;
using Sharpen;

namespace Org.Apache.Http.Config
{
	/// <summary>
	/// Builder for
	/// <see cref="Registry{I}">Registry&lt;I&gt;</see>
	/// instances.
	/// </summary>
	/// <since>4.3</since>
	public sealed class RegistryBuilder<I>
	{
		private readonly IDictionary<string, I> items;

		public static Org.Apache.Http.Config.RegistryBuilder<I> Create<I>()
		{
			return new Org.Apache.Http.Config.RegistryBuilder<I>();
		}

		internal RegistryBuilder() : base()
		{
			this.items = new Dictionary<string, I>();
		}

		public Org.Apache.Http.Config.RegistryBuilder<I> Register(string id, I item)
		{
            Args.NotEmpty(id, "ID");
			Args.NotNull(item, "Item");
			items.Put(id.ToLower(CultureInfo.InvariantCulture), item);
			return this;
		}

		public Registry<I> Build()
		{
			return new Registry<I>(items);
		}

		public override string ToString()
		{
			return items.ToString();
		}
	}
}
