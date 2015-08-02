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

namespace Owasp.Esapi.Errors
{
    /// <summary>
    ///     An EncryptionException should be thrown for any problems related to
    ///     encryption, hashing, or digital signatures.
    /// </summary>
    [Serializable]
    public class EncryptionException : EnterpriseSecurityException
    {
        /// <summary> Instantiates a new EncryptionException.</summary>
        protected internal EncryptionException()
        {
            // hidden
        }

        /// <summary>
        ///     Creates a new instance of EncryptionException.
        /// </summary>
        /// <param name="userMessage">
        ///     The message for the user.
        /// </param>
        /// <param name="logMessage">
        ///     The message for the log.
        /// </param>
        public EncryptionException(string userMessage, string logMessage)
            : base(userMessage, logMessage)
        {
        }

        /// <summary>
        ///     Instantiates a new EncryptionException.
        /// </summary>
        /// <param name="userMessage">
        ///     The message for the user.
        /// </param>
        /// <param name="logMessage">
        ///     The message for the log.
        /// </param>
        /// <param name="cause">
        ///     The cause of the exception.
        /// </param>
        public EncryptionException(string userMessage, string logMessage, Exception cause)
            : base(userMessage, logMessage, cause)
        {
        }
    }
}
