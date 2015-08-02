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

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     ESAPI action arguments
    /// </summary>
    [Serializable]
    public class ActionArgs
    {
        /// <summary>
        ///     Emtpy action arguments
        /// </summary>
        public static readonly ActionArgs Empty = new ActionArgs();

        private Exception _faultException;

        private IRule _faultingRule;

        private RuntimeEventArgs _runtimeEventArgs;

        /// <summary>
        ///     Faulting rule
        /// </summary>
        public IRule FaultingRule
        {
            get
            {
                return this._faultingRule;
            }
            internal set
            {
                this._faultingRule = value;
            }
        }

        /// <summary>
        ///     Fault exception
        /// </summary>
        public Exception FaultException
        {
            get
            {
                return this._faultException;
            }
            internal set
            {
                this._faultException = value;
            }
        }

        /// <summary>
        ///     Runtime event arguments
        /// </summary>
        public RuntimeEventArgs RuntimeArgs
        {
            get
            {
                return this._runtimeEventArgs;
            }
            internal set
            {
                this._runtimeEventArgs = value;
            }
        }
    }
}
