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

namespace Owasp.Esapi.Runtime.Actions
{
    /// <summary>
    ///     ESAPI builtin intrusion detection actions
    /// </summary>
    public static class BuiltinActions
    {
        /// <summary>
        ///     Log action
        /// </summary>
        public const string Log = "Log";

        /// <summary>
        ///     Disable ASP.NET Membership account
        /// </summary>
        public const string MembershipDisable = "MembershipDisable";

        /// <summary>
        ///     Do Forms authentication logout
        /// </summary>
        public const string FormsAuthenticationLogout = "FormsAuthenticationLogout";

        /// <summary>
        ///     Block HTTP request
        /// </summary>
        public const string Block = "Block";

        /// <summary>
        ///     Redirect user to page
        /// </summary>
        public const string Redirect = "Redirect";

        /// <summary>
        ///     Transfer user to page
        /// </summary>
        public const string Transfer = "Transfer";
    }
}
