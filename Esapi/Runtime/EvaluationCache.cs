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

using System.Collections.Generic;

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     Evaluation cache
    /// </summary>
    internal class EvaluationCache<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _values;

        /// <summary>
        ///     Initialize cache
        /// </summary>
        public EvaluationCache()
        {
            this._values = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        ///     Get cached value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>True if found, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._values.TryGetValue(key, out value);
        }

        /// <summary>
        ///     Cache value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetValue(TKey key, TValue value)
        {
            this._values[key] = value;
        }
    }
}
