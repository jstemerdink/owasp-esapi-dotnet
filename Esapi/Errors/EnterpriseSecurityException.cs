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

namespace Owasp.Esapi.Errors
{
    /// <summary>
    ///     EnterpriseSecurityException is the base class for all security related exceptions. You should pass in the root
    ///     cause
    ///     exception where possible. Constructors for classes extending EnterpriseSecurityException should be sure to call the
    ///     appropriate base constructor in order to ensure that logging and intrusion detection occur properly.
    /// </summary>
    [Serializable]
    public class EnterpriseSecurityException : Exception
    {
        /// <summary>The logger. </summary>
        private static readonly ILogger logger;

        /// <summary>The message for the log. </summary>
        private readonly string _logMessage;

        static EnterpriseSecurityException()
        {
            logger = Esapi.Logger;
        }

        /// <summary> Instantiates a new security exception.</summary>
        protected internal EnterpriseSecurityException()
        {
            // hidden
        }

        /// <summary>
        ///     Creates a new instance of EnterpriseSecurityException. This exception is automatically logged, so that simply by
        ///     using this API, applications will generate an extensive security log. In addition, this exception is
        ///     automatically registrered with the IntrusionDetector, so that quotas can be checked.
        /// </summary>
        /// <param name="userMessage">
        ///     The message for the user.
        /// </param>
        /// <param name="logMessage">
        ///     The message for the log.
        /// </param>
        public EnterpriseSecurityException(string userMessage, string logMessage)
            : base(userMessage)
        {
            this._logMessage = logMessage;
            Esapi.IntrusionDetector.AddException(this);
        }

        /// <summary>
        ///     Creates a new instance of EnterpriseSecurityException that includes a root cause Throwable.
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
        public EnterpriseSecurityException(string userMessage, string logMessage, Exception cause)
            : base(userMessage, cause)
        {
            this._logMessage = logMessage;
            Esapi.IntrusionDetector.AddException(this);
        }

        /// <summary>
        ///     The message for the user
        /// </summary>
        public virtual string UserMessage
        {
            get
            {
                return this.Message;
            }
        }

        /// <summary>
        ///     The message for the log
        /// </summary>
        public virtual string LogMessage
        {
            get
            {
                return this._logMessage;
            }
        }
    }
}
