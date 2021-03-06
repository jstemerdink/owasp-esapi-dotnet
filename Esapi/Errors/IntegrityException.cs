﻿// Copyright© 2015 OWASP.org. 
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
    ///     An IntegrityException should be thrown when a problem with the integrity of data
    ///     has been detected. For example, if a financial account cannot be reconciled after
    ///     a transaction has been performed, an integrity exception should be thrown.
    /// </summary>
    [Serializable]
    public class IntegrityException : EnterpriseSecurityException
    {
        /// <summary> Instantiates a new integrity exception.</summary>
        protected internal IntegrityException()
        {
            // hidden
        }

        /// <summary>
        ///     Creates a new instance of IntegrityException.
        /// </summary>
        /// <param name="userMessage">
        ///     The message for the user.
        /// </param>
        /// <param name="logMessage">
        ///     The message for the log.
        /// </param>
        public IntegrityException(string userMessage, string logMessage)
            : base(userMessage, logMessage)
        {
        }

        /// <summary>
        ///     Instantiates a new IntegrityException.
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
        public IntegrityException(string userMessage, string logMessage, Exception cause)
            : base(userMessage, logMessage, cause)
        {
        }
    }
}
