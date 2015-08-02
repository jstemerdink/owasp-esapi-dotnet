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
    ///     Runtime context condition
    /// </summary>
    internal class ContextCondition : IContextCondition
    {
        private readonly ICondition _condition;

        private bool _result;

        public ContextCondition(ICondition condition)
            : this(condition, true)
        {
        }

        /// <summary>
        ///     Initialize condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="result"></param>
        public ContextCondition(ICondition condition, bool result)
        {
            if (condition == null)
            {
                throw new ArgumentNullException();
            }
            this._condition = condition;
            this._result = result;
        }

        #region IContextCondition implementation

        public ICondition Condition
        {
            get
            {
                return this._condition;
            }
        }

        public bool Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }

        #endregion
    }
}
