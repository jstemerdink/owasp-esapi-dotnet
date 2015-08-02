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
using System.Web.UI;

namespace Owasp.Esapi.Runtime.Rules
{
    /// <summary>
    ///     Intrusion detection CSRF rule
    /// </summary>
    public class CsrfRule : IRule
    {
        /// <summary>
        ///     Verify CSRF guard before page executes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreRequestHandlerExecute(object sender, RuntimeEventArgs e)
        {
            // Get current page
            Page currentPage = (HttpContext.Current != null ? HttpContext.Current.CurrentHandler as Page : null);

            if (currentPage != null)
            {
                // Add CSRF guard when page initializes
                currentPage.Init += (p, a) => Esapi.HttpUtilities.AddCsrfToken();
            }
        }

        #region IRule Members

        /// <summary>
        ///     Subscribe to events
        /// </summary>
        /// <param name="publisher"></param>
        public void Subscribe(IRuntimeEventPublisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException();
            }
            publisher.PreRequestHandlerExecute += this.OnPreRequestHandlerExecute;
        }

        /// <summary>
        ///     Disconnect from events
        /// </summary>
        /// <param name="publisher"></param>
        public void Unsubscribe(IRuntimeEventPublisher publisher)
        {
            if (publisher == null)
            {
                throw new ArgumentNullException();
            }

            publisher.PreRequestHandlerExecute -= this.OnPreRequestHandlerExecute;
        }

        #endregion
    }
}
