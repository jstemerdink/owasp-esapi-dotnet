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
using System.Text;

using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi.Codecs
{
    /// <summary>
    ///     This class performs Base64 encoding and decoding.
    /// </summary>
    [Codec(BuiltinCodecs.Base64)]
    public class Base64Codec : ICodec
    {
        #region ICodec Members

        /// <summary>
        ///     Encode the input to a Base64 value.
        /// </summary>
        /// <param name="input">The string to encode.</param>
        /// <returns>The encoded string.</returns>
        public string Encode(string input)
        {
            byte[] inputBytes = Encoding.GetEncoding(Esapi.SecurityConfiguration.CharacterEncoding).GetBytes(input);
            return Convert.ToBase64String(inputBytes);
        }

        /// <summary>
        ///     Decode the input from a Base64 value.
        /// </summary>
        /// <param name="input">The string to decode/</param>
        /// <returns>The decoded string.</returns>
        public string Decode(string input)
        {
            byte[] inputBytes = Convert.FromBase64String(input);
            return Encoding.GetEncoding(Esapi.SecurityConfiguration.CharacterEncoding).GetString(inputBytes);
        }

        #endregion
    }
}
