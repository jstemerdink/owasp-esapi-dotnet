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

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder" />
    /// <summary>
    ///     Reference implementation of the IEncoder interface.
    /// </summary>
    public class Encoder : IEncoder
    {
        private readonly Dictionary<string, ICodec> codecs = new Dictionary<string, ICodec>();

        private readonly ILogger logger = Esapi.Logger;

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.Canonicalize(string, bool)" />
        public string Canonicalize(string input, bool strict)
        {
            return this.Canonicalize(this.codecs.Keys, input, strict);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.Canonicalize(IEnumerable&lt;string&gt;, string, bool)" />
        public string Canonicalize(IEnumerable<string> codecNames, string input, bool strict)
        {
            if (codecNames == null)
            {
                throw new ArgumentNullException("codecNames");
            }
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            string working = input;
            ICodec codecFound = null;
            int mixedCount = 1;
            int foundCount = 0;
            bool clean = false;
            while (!clean)
            {
                clean = true;
                // try each codec and keep track of which ones work             
                foreach (string codecName in codecNames)
                {
                    if (string.IsNullOrEmpty(codecName))
                    {
                        continue;
                    }

                    string old = working;
                    ICodec codec = this.codecs[codecName];
                    working = codec.Decode(working);
                    if (!old.Equals(working))
                    {
                        if (codecFound != null && codecFound != codec)
                        {
                            mixedCount++;
                        }
                        codecFound = codec;
                        if (clean)
                        {
                            foundCount++;
                        }
                        clean = false;
                    }
                }
            }
            // do strict tests and handle if any mixed, multiple, nested encoding were found 
            if (foundCount >= 2 && mixedCount > 1)
            {
                if (strict)
                {
                    throw new IntrusionException(
                        EM.Encoder_InputValidationFailure,
                        string.Format(EM.Encoder_MultipleMixedEncoding2, foundCount, mixedCount));
                }
                this.logger.Warning(
                    LogEventTypes.SECURITY,
                    string.Format(EM.Encoder_MultipleMixedEncoding2, foundCount, mixedCount));
            }
            else if (foundCount >= 2)
            {
                if (strict)
                {
                    throw new IntrusionException(
                        EM.Encoder_InputValidationFailure,
                        string.Format(EM.Encoder_MultipleEncoding1, foundCount));
                }
                this.logger.Warning(LogEventTypes.SECURITY, string.Format(EM.Encoder_MultipleEncoding1, foundCount));
            }
            else if (mixedCount > 1)
            {
                if (strict)
                {
                    throw new IntrusionException(
                        EM.Encoder_InputValidationFailure,
                        string.Format(EM.Encoder_MixedEncoding1, mixedCount));
                }
                this.logger.Warning(LogEventTypes.SECURITY, string.Format(EM.Encoder_MixedEncoding1, mixedCount));
            }
            return working;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.Normalize(string)" />
        public string Normalize(string input)
        {
            return input.Normalize();
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.Encode(string, string)" />
        public string Encode(string codecName, string input)
        {
            ICodec codec = this.GetCodec(codecName);
            if (codec == null)
            {
                throw new ArgumentOutOfRangeException("codecName");
            }

            return codec.Encode(input);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.Decode(string, string)" />
        public string Decode(string codecName, string input)
        {
            ICodec codec = this.GetCodec(codecName);
            if (codec == null)
            {
                throw new ArgumentOutOfRangeException("codecName");
            }

            return codec.Decode(input);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.GetCodec(string)" />
        public ICodec GetCodec(string codecName)
        {
            if (codecName == null)
            {
                throw new ArgumentNullException("codecName");
            }

            ICodec codec;
            this.codecs.TryGetValue(codecName, out codec);
            return codec;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.AddCodec(string, ICodec)" />
        public void AddCodec(string codecName, ICodec codec)
        {
            if (codecName == null)
            {
                throw new ArgumentNullException("codecName");
            }
            this.codecs.Add(codecName, codec);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IEncoder.RemoveCodec(string)" />
        public void RemoveCodec(string codecName)
        {
            this.codecs.Remove(codecName);
        }
    }
}
