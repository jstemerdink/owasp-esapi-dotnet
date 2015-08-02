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

namespace Owasp.Esapi.Runtime.Rules
{
    /// <summary>
    ///     Clickjack detection rule
    /// </summary>
    public class ClickjackRule : IRule
    {
        /// <summary>
        ///     Framing mode
        /// </summary>
        public enum FramingModeType
        {
            /// <summary>
            ///     Deny framing
            /// </summary>
            Deny,

            /// <summary>
            ///     Allow only same domain
            /// </summary>
            Sameorigin
        }

        private const string HeaderName = "X-FRAME-OPTIONS";

        private const string DenyValue = "DENY";

        private const string SameoriginValue = "SAMEORIGIN";

        private FramingModeType _mode;

        /// <summary>
        ///     Initialize clickjack rule
        /// </summary>
        public ClickjackRule()
        {
            this._mode = FramingModeType.Deny;
        }

        /// <summary>
        ///     Initialize clickjack rule
        /// </summary>
        /// <param name="mode">Framing mode type</param>
        public ClickjackRule(FramingModeType mode)
        {
            this._mode = mode;
        }

        /// <summary>
        ///     Framing mode type
        /// </summary>
        public FramingModeType FramingMode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
            }
        }

        /// <summary>
        ///     Add clickjack headers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPostRequestHandlerExecute(object sender, RuntimeEventArgs e)
        {
            // Get response
            HttpResponse response = (HttpContext.Current != null ? HttpContext.Current.Response : null);
            if (response == null)
            {
                throw new InvalidOperationException();
            }

            // Add clickjack protection
            switch (this._mode)
            {
                case FramingModeType.Deny:
                    response.AddHeader(HeaderName, DenyValue);
                    break;
                case FramingModeType.Sameorigin:
                    response.AddHeader(HeaderName, SameoriginValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            publisher.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
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
            publisher.PostRequestHandlerExecute -= this.OnPostRequestHandlerExecute;
        }

        #endregion
    }
}
