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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The ICodec interface defines a set of methods for encoding and decoding application level encoding schemes,
    ///     such as HTML entity encoding and percent encoding (aka URL encoding).
    /// </summary>
    public interface ICodec
    {
        /// <summary>
        ///     Decode a String that was encoded using the encode method in this Class.
        /// </summary>
        /// <param name="input">The string to decode.</param>
        /// <returns>The decoded string.</returns>
        string Encode(string input);

        /// <summary>
        ///     Decode a String that was encoded using the encode method in this Class.
        /// </summary>
        /// <param name="input">The string to decode.</param>
        /// <returns>The decoded string.</returns>
        string Decode(string input);
    }
}
