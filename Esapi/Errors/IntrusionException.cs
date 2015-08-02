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
    ///     An IntrusionException should be thrown anytime an error condition arises that is likely to be the result of an
    ///     attack
    ///     in progress. IntrusionExceptions are handled specially by the IntrusionDetector, which is equipped to respond by
    ///     either specially logging the event, logging out the current user, or invalidating the current user's account.
    /// </summary>
    [Serializable]
    public class IntrusionException : Exception
    {
        /// <summary>The logger. </summary>
        private static readonly ILogger logger;

        /// <summary>
        ///     The message for the log
        /// </summary>
        private readonly string _logMessage;

        static IntrusionException()
        {
            logger = Esapi.Logger;
        }

        /// <summary>
        ///     Internal classes may throw an IntrusionException to the IntrusionDetector, which generates the appropriate log
        ///     message.
        /// </summary>
        public IntrusionException()
        {
        }

        /// <summary>
        ///     Creates a new instance of IntrusionException.
        /// </summary>
        /// <param name="userMessage">
        ///     The message for the user.
        /// </param>
        /// <param name="logMessage">
        ///     The message for the log.
        /// </param>
        public IntrusionException(string userMessage, string logMessage)
            : base(userMessage)
        {
            this._logMessage = logMessage;
            logger.Error(LogEventTypes.SECURITY, "INTRUSION - " + logMessage);
        }

        /// <summary>
        ///     Instantiates a new intrusion exception.
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
        public IntrusionException(string userMessage, string logMessage, Exception cause)
            : base(userMessage, cause)
        {
            this._logMessage = logMessage;
            logger.Error(LogEventTypes.SECURITY, "INTRUSION - " + logMessage, cause);
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
