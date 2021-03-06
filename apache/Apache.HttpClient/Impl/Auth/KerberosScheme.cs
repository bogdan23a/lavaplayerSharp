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
using Apache.Http.Auth;
using Apache.Http.Impl.Auth;
using Apache.Http.Protocol;
using Apache.Http.Util;
using Org.Apache.Http;
using Sharpen;

namespace Apache.Http.Impl.Auth
{
	/// <summary>KERBEROS authentication scheme.</summary>
	/// <remarks>KERBEROS authentication scheme.</remarks>
	/// <since>4.2</since>
	public class KerberosScheme : GGSSchemeBase
	{
		private const string KerberosOid = "1.2.840.113554.1.2.2";

		public KerberosScheme(bool stripPort) : base(stripPort)
		{
		}

		public KerberosScheme() : base(false)
		{
		}

		public override string GetSchemeName()
		{
			return "Kerberos";
		}

		/// <summary>
		/// Produces KERBEROS authorization Header based on token created by
		/// processChallenge.
		/// </summary>
		/// <remarks>
		/// Produces KERBEROS authorization Header based on token created by
		/// processChallenge.
		/// </remarks>
		/// <param name="credentials">not used by the KERBEROS scheme.</param>
		/// <param name="request">The request being authenticated</param>
		/// <exception cref="Apache.Http.Auth.AuthenticationException">
		/// if authentication string cannot
		/// be generated due to an authentication failure
		/// </exception>
		/// <returns>KERBEROS authentication Header</returns>
		public override Header Authenticate(Credentials credentials, IHttpRequest request
			, HttpContext context)
		{
			return base.Authenticate(credentials, request, context);
		}

		/// <exception cref="Sharpen.GSSException"></exception>
		protected internal override byte[] GenerateToken(byte[] input, string authServer)
		{
			return GenerateGSSToken(input, new Oid(KerberosOid), authServer);
		}

		/// <summary>
		/// There are no valid parameters for KERBEROS authentication so this
		/// method always returns <code>null</code>.
		/// </summary>
		/// <remarks>
		/// There are no valid parameters for KERBEROS authentication so this
		/// method always returns <code>null</code>.
		/// </remarks>
		/// <returns><code>null</code></returns>
		public override string GetParameter(string name)
		{
			Args.NotNull(name, "Parameter name");
			return null;
		}

		/// <summary>
		/// The concept of an authentication realm is not supported by the Negotiate
		/// authentication scheme.
		/// </summary>
		/// <remarks>
		/// The concept of an authentication realm is not supported by the Negotiate
		/// authentication scheme. Always returns <code>null</code>.
		/// </remarks>
		/// <returns><code>null</code></returns>
		public override string GetRealm()
		{
			return null;
		}

		/// <summary>Returns <tt>true</tt>.</summary>
		/// <remarks>Returns <tt>true</tt>. KERBEROS authentication scheme is connection based.
		/// 	</remarks>
		/// <returns><tt>true</tt>.</returns>
		public override bool IsConnectionBased()
		{
			return true;
		}
	}
}
