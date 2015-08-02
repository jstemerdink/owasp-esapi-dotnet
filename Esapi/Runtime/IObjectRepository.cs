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
    ///     Object repository
    /// </summary>
    public interface IObjectRepository<TId, TObject>
        where TObject : class
    {
        /// <summary>
        ///     Get object count
        /// </summary>
        int Count { get; }

        /// <summary>
        ///     Get ids
        /// </summary>
        ICollection<TId> Ids { get; }

        /// <summary>
        ///     Get objects
        /// </summary>
        ICollection<TObject> Objects { get; }

        /// <summary>
        ///     Register object
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Generated ID</returns>
        TId Register(TObject value);

        /// <summary>
        ///     Add object
        /// </summary>
        /// <param id="id">Object id</param>
        /// <param id="value">Object value</param>
        /// <returns></returns>
        void Register(TId id, TObject value);

        /// <summary>
        ///     Remove object
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        void Revoke(TId id);

        /// <summary>
        ///     Lookup object
        /// </summary>
        /// <param id="id"></param>
        /// <param id="?"></param>
        /// <returns></returns>
        bool Lookup(TId id, out TObject value);

        /// <summary>
        ///     Get object
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        TObject Get(TId id);
    }
}
