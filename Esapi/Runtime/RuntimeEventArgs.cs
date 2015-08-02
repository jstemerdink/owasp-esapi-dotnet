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

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     Runtime event arguments
    /// </summary>
    public class RuntimeEventArgs : EventArgs
    {
        private readonly EvaluationCache<ICondition, bool> _conditionEvalCache;

        private readonly EvaluationCache<IContext, bool> _contextMatchCache;

        /// <summary>
        ///     Context stack
        /// </summary>
        private readonly Stack<IContext> _contexts;

        /// <summary>
        ///     Initialize runtime arguments
        /// </summary>
        public RuntimeEventArgs()
        {
            this._contexts = new Stack<IContext>();
            this._contextMatchCache = new EvaluationCache<IContext, bool>();
            this._conditionEvalCache = new EvaluationCache<ICondition, bool>();
        }

        /// <summary>
        ///     Get context path
        /// </summary>
        public IEnumerable<IContext> ContextPath
        {
            get
            {
                return this._contexts;
            }
        }

        /// <summary>
        ///     Get current context
        /// </summary>
        public IContext CurrentContext
        {
            get
            {
                return (this._contexts.Count > 0 ? this._contexts.Peek() : null);
            }
        }

        #region Internal

        /// <summary>
        ///     Push context
        /// </summary>
        /// <param name="context">
        ///     A <see cref="IContext" />
        /// </param>
        internal void PushContext(IContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException();
            }
            this._contexts.Push(context);
        }

        /// <summary>
        ///     Pop context
        /// </summary>
        /// <returns>
        ///     A <see cref="IContext" />
        /// </returns>
        internal IContext PopContext()
        {
            return this._contexts.Pop();
        }

        /// <summary>
        ///     Context match cache
        /// </summary>
        internal EvaluationCache<IContext, bool> MatchCache
        {
            get
            {
                return this._contextMatchCache;
            }
        }

        /// <summary>
        ///     Condition evaluation cache
        /// </summary>
        internal EvaluationCache<ICondition, bool> EvalCache
        {
            get
            {
                return this._conditionEvalCache;
            }
        }

        #endregion
    }
}
