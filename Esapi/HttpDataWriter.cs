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
using System.Collections.Specialized;
using System.IO;

namespace Owasp.Esapi
{
    /// <summary>
    ///     HTTP data writer
    /// </summary>
    internal class HttpDataWriter : IDisposable
    {
        private readonly TextWriter _output;

        private bool _hasValues;

        private bool _insideSection;

        protected HttpDataWriter(TextWriter output)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            this._output = output;
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Write header
        /// </summary>
        /// <param name="text"></param>
        protected void WriteHeader(string text)
        {
            this._output.Write(text + ": ");
        }

        /// <summary>
        ///     Write footer
        /// </summary>
        protected void WriteFooter()
        {
            if (this._insideSection)
            {
                this._output.Write(") ");
            }
        }

        /// <summary>
        ///     Start new data section
        /// </summary>
        /// <param name="name"></param>
        protected void WriteSection(string name)
        {
            if (this._insideSection)
            {
                this._output.Write(") ");
                this._insideSection = false;
                this._hasValues = false;
            }

            this._output.Write(" (" + name + ": ");
            this._insideSection = !string.IsNullOrEmpty(name);
        }

        /// <summary>
        ///     Write value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        protected void WriteValue(string name, string value)
        {
            this._output.Write("{0}\"{1}\"=\"{2}\"", this._hasValues ? ", " : "", name, value);
            this._hasValues = true;
        }

        /// <summary>
        ///     Write value collection
        /// </summary>
        /// <param name="values"></param>
        protected void WriteValues(NameValueCollection values)
        {
            if (values != null)
            {
                foreach (string name in values.Keys)
                {
                    this.WriteValue(name, values[name]);
                }
            }
        }

        /// <summary>
        ///     Write value
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="obfuscatedValues">Parameter names whose values are obfuscated</param>
        protected void WriteObfuscatedValue(string name, string value, ICollection<string> obfuscatedValues)
        {
            string obfuscatedValue = value;
            if (obfuscatedValues != null && obfuscatedValues.Contains(name))
            {
                obfuscatedValue = "********";
            }
            this.WriteValue(name, obfuscatedValue);
        }

        /// <summary>
        ///     Write values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="obfuscatedValues">Parameter names whose values are obfuscated</param>
        protected void WriteObfuscatedValues(NameValueCollection values, ICollection<string> obfuscatedValues)
        {
            if (values != null)
            {
                foreach (string name in values.Keys)
                {
                    this.WriteObfuscatedValue(name, values[name], obfuscatedValues);
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._output.Dispose();
            }
        }

        // Disposable types implement a finalizer.
        ~HttpDataWriter()
        {
            this.Dispose(false);
        }
    }
}
