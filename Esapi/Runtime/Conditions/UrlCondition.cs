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
using System.Text.RegularExpressions;
using System.Web;

namespace Owasp.Esapi.Runtime.Conditions
{
    /// <summary>
    ///     Regex based URL context condition
    /// </summary>
    public class UrlCondition : ICondition
    {
        /// <summary>
        ///     Any URL pattern
        /// </summary>
        private const string AnyUrlPattern = "*";

        private Regex _url;

        /// <summary>
        ///     Intialize URL condition
        /// </summary>
        /// <param name="urlPattern">URL Pattern</param>
        public UrlCondition(string urlPattern)
        {
            this.UrlPattern = urlPattern;
        }

        /// <summary>
        ///     URL pattern
        /// </summary>
        public string UrlPattern
        {
            get
            {
                return this._url.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._url = new Regex("^$");
                }
                else
                {
                    this._url = new Regex(value);
                }
            }
        }

        #region ICondition Members

        /// <summary>
        ///     Verify URL condition
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Evaluate(ConditionArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException();
            }

            HttpRequest request = HttpContext.Current != null ? HttpContext.Current.Request : null;
            if (request != null)
            {
                return this._url.IsMatch(request.Url.ToString());
            }
            return false;
        }

        #endregion
    }
}
