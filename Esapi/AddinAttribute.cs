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

namespace Owasp.Esapi
{
    /// <summary>
    ///     Named addin attribute
    /// </summary>
    public abstract class AddinAttribute : Attribute
    {
        private readonly string _name;

        private bool _autoLoad;

        /// <summary>
        ///     Initialize addin attribute
        /// </summary>
        /// <param name="name">Addin unique name</param>
        protected AddinAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name");
            }
            this._name = name;
            this._autoLoad = true;
        }

        /// <summary>
        ///     Addin unique name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        ///     Addin can be loaded automatically
        /// </summary>
        /// <remarks>
        ///     Set to false if the addin requires initialization parameters
        /// </remarks>
        public bool AutoLoad
        {
            get
            {
                return this._autoLoad;
            }
            set
            {
                this._autoLoad = value;
            }
        }
    }
}
