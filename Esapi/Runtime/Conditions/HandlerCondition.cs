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

namespace Owasp.Esapi.Runtime.Conditions
{
    /// <summary>
    ///     Handler match condition
    /// </summary>
    public class HandlerCondition : ICondition
    {
        private Type _handlerType;

        /// <summary>
        ///     Initialize handler condition
        /// </summary>
        public HandlerCondition()
        {
            this._handlerType = null;
        }

        /// <summary>
        ///     Initialize handler condition
        /// </summary>
        /// <param name="handlerType">Handler type to match</param>
        public HandlerCondition(Type handlerType)
        {
            if (handlerType == null)
            {
                throw new ArgumentNullException();
            }
            this._handlerType = handlerType;
        }

        /// <summary>
        ///     Handler type to match
        /// </summary>
        public Type HandlerType
        {
            get
            {
                return this._handlerType;
            }
            set
            {
                this._handlerType = value;
            }
        }

        #region ICondition Members

        /// <summary>
        ///     Evaluate handler condition
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool Evaluate(ConditionArgs args)
        {
            bool isMatch = false;

            IHttpHandler handler = (HttpContext.Current != null ? HttpContext.Current.CurrentHandler : null);

            if (handler != null && this._handlerType != null)
            {
                isMatch = handler.GetType().Equals(this._handlerType);
            }

            return isMatch;
        }

        #endregion
    }
}
