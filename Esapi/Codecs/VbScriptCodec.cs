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

using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi.Codecs
{
    /// <summary>
    ///     This class performs VBScript encoding. This is useful for encoding values that will be displayed in a browser
    ///     as JavaScript  (i.e. &lt;script type="VbScript" &gt; "untrusted data here" &lt;/script&gt;)
    /// </summary>
    [Codec(BuiltinCodecs.VbScript)]
    public class VbScriptCodec : ICodec
    {
        #region ICodec Members

        /// <summary>
        ///     VBScript encode the input.
        /// </summary>
        /// <param name="input">The input to encode.</param>
        /// <returns>The encoded input.</returns>
        public string Encode(string input)
        {
            return Microsoft.Security.Application.Encoder.VisualBasicScriptEncode(input);
        }

        /// <summary>
        ///     VbScript decode the input.
        /// </summary>
        /// <param name="input">The input to decode.</param>
        /// <returns>The decoded input.</returns>
        /// <remarks>This method is not implemented.</remarks>
        public string Decode(string input)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
