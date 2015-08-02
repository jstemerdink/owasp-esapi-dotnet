// Copyright© 2015 OWASP.org. 
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Web;

namespace Owasp.Esapi
{
    /// <summary>
    ///     HTTP request writer
    /// </summary>
    internal class HttpRequestWriter : HttpDataWriter
    {
        internal HttpRequestWriter(TextWriter output)
            : base(output)
        {
        }

        /// <summary>
        ///     Write HTTP request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <param name="obfuscatedParams"></param>
        /// <param name="verbose"></param>
        internal void Write(HttpRequest request, IPrincipal user, ICollection<string> obfuscatedParams, bool verbose)
        {
            if (request == null)
            {
                throw new ArgumentNullException("user");
            }

            IPrincipal userPrincipal = (user == null ? Esapi.SecurityConfiguration.CurrentUser : user);

            //
            this.WriteHeader("HttpRequestData");

            // User
            this.WriteSection("User");
            this.WriteValue("Identity", (userPrincipal != null ? userPrincipal.Identity.Name : "<not set>"));
            this.WriteValue("HostName", request.UserHostName);
            this.WriteValue("HostAddress", request.UserHostAddress);
            this.WriteValue("IsAuthenticated", request.IsAuthenticated.ToString());

            // Request
            this.WriteSection("Request");
            this.WriteValue("RawUrl", request.RawUrl);
            this.WriteValue("HttpMethod", request.HttpMethod);
            this.WriteValue("IsSecure", request.IsSecureConnection.ToString());

            // Cookies
            this.WriteSection("Cookies");
            foreach (HttpCookie cookie in request.Cookies)
            {
                this.WriteValue(cookie.Name, cookie.ToString());
            }

            // Headers
            this.WriteSection("Headers");
            this.WriteValues(request.Headers);

            // Form 
            this.WriteSection("Form");
            this.WriteObfuscatedValues(request.Form, obfuscatedParams);

            // Params
            this.WriteSection("Params");
            this.WriteObfuscatedValues(request.Params, obfuscatedParams);

            if (verbose)
            {
                // Server variables
                this.WriteSection("ServerVariables");
                this.WriteObfuscatedValues(request.ServerVariables, obfuscatedParams);
            }

            // Done
            this.WriteFooter();
        }
    }
}
