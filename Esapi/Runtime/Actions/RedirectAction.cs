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
using System.Web;

namespace Owasp.Esapi.Runtime.Actions
{
    /// <summary>
    ///     Redirect request
    /// </summary>
    [Action(BuiltinActions.Redirect, AutoLoad = false)]
    internal class RedirectAction : IAction
    {
        private string _url;

        /// <summary>
        ///     Initialize redirect action
        /// </summary>
        /// <param name="url">Url to redirect to</param>
        public RedirectAction(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException();
            }

            this._url = url;
        }

        /// <summary>
        ///     Redirect URL
        /// </summary>
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                this._url = value;
            }
        }

        #region IAction Members

        /// <summary>
        ///     Execute redirect action
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>Will terminate the current request</remarks>
        public void Execute(ActionArgs args)
        {
            HttpResponse response = (HttpContext.Current != null ? HttpContext.Current.Response : null);
            if (response == null)
            {
                throw new InvalidOperationException();
            }

            response.Redirect(this._url, true);
        }

        #endregion
    }
}
