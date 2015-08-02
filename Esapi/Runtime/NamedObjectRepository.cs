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
    ///     Object repository
    /// </summary>
    /// <typeparam id="TObject"></typeparam>
    internal class NamedObjectRepository<TObject> : IObjectRepository<string, TObject>
        where TObject : class
    {
        private readonly Dictionary<string, TObject> _entries;

        public NamedObjectRepository()
        {
            this._entries = new Dictionary<string, TObject>();
        }

        public NamedObjectRepository(IDictionary<string, TObject> entries)
        {
            this._entries = (entries != null
                                 ? new Dictionary<string, TObject>(entries)
                                 : new Dictionary<string, TObject>());
        }

        /// <summary>
        ///     Get entries
        /// </summary>
        protected Dictionary<string, TObject> Entries
        {
            get
            {
                return this._entries;
            }
        }

        #region IObjectRepository<TName, TObject> Members

        /// <summary>
        ///     Register object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Register(TObject value)
        {
            string id = Guid.NewGuid().ToString();
            this.Register(id, value);
            return id;
        }

        /// <summary>
        ///     Register object
        /// </summary>
        /// <param id="id"></param>
        /// <param id="value"></param>
        /// <returns></returns>
        public void Register(string id, TObject value)
        {
            if (string.IsNullOrEmpty(id) || this._entries.ContainsKey(id))
            {
                throw new ArgumentException("Invalid id", "id");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this._entries[id] = value;
        }

        /// <summary>
        ///     Revoke object
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public void Revoke(string id)
        {
            this._entries.Remove(id);
        }

        /// <summary>
        ///     Lookup object
        /// </summary>
        /// <param id="id"></param>
        /// <param id="value"></param>
        /// <returns></returns>
        public bool Lookup(string id, out TObject value)
        {
            return this._entries.TryGetValue(id, out value);
        }

        /// <summary>
        ///     Get count
        /// </summary>
        public int Count
        {
            get
            {
                return this._entries.Count;
            }
        }

        /// <summary>
        ///     Get keys
        /// </summary>
        public ICollection<string> Ids
        {
            get
            {
                return this._entries.Keys;
            }
        }

        /// <summary>
        ///     Get objects
        /// </summary>
        public ICollection<TObject> Objects
        {
            get
            {
                return this._entries.Values;
            }
        }

        /// <summary>
        ///     Get object
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        public TObject Get(string id)
        {
            return this._entries[id];
        }

        #endregion
    }
}
