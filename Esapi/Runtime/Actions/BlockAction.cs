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
    ///     Block current request action
    /// </summary>
    [Action(BuiltinActions.Block)]
    public class BlockAction : IAction
    {
        private int _statusCode = 403; //Forbidden

        /// <summary>
        ///     Block HTTP status code
        /// </summary>
        public int StatusCode
        {
            get
            {
                return this._statusCode;
            }
            set
            {
                this._statusCode = value;
            }
        }

        #region IAction Members

        /// <summary>
        ///     Block current request
        /// </summary>
        /// <param name="args"></param>
        /// <remarks>Will end the current request</remarks>
        public void Execute(ActionArgs args)
        {
            HttpResponse response = (HttpContext.Current != null ? HttpContext.Current.Response : null);

            if (null == response)
            {
                throw new InvalidOperationException();
            }

            response.ClearHeaders();
            response.ClearContent();

            response.StatusCode = this._statusCode;
            response.End();
        }

        #endregion
    }
}
