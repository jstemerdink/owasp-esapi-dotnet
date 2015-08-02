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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The ILogger interface defines a set of methods that can be used to log events.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     The logging level associated with this Logger.
        /// </summary>
        int Level { get; set; }

        /// <summary> Log a fatal event if 'fatal' level logging is enabled.</summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        void Fatal(int type, string message);

        /// <summary>
        ///     Log a fatal level security event if 'fatal' level logging is enabled
        ///     and also record the stack trace associated with the event.
        /// </summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        /// <param name="exception">
        ///     The exception to log.
        /// </param>
        void Fatal(int type, string message, Exception exception);

        /// <summary>
        ///     Allows the caller to determine if messages logged at this level
        ///     will be discarded, to avoid performing expensive processing.
        /// </summary>
        /// <returns>true, if fatal level messages will be output to the log.</returns>
        bool IsFatalEnabled();

        /// <summary>Log an error level security event if 'error' level logging is enabled.</summary>
        /// <param name="type">
        ///     The type of event
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        void Error(int type, string message);

        /// <summary>
        ///     Log an error level security event if 'error' level logging is enabled
        ///     and also record the stack trace associated with the event.
        /// </summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        /// <param name="throwable">
        ///     The exception to log.
        /// </param>
        void Error(int type, string message, Exception throwable);

        /// <summary>
        ///     Allows the caller to determine if messages logged at this level
        ///     will be discarded, to avoid performing expensive processing.
        /// </summary>
        /// <returns>true, if error level messages will be output to the log.</returns>
        bool IsErrorEnabled();

        /// <summary> Log a warning level security event if 'warning' level logging is enabled.</summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        void Warning(int type, string message);

        /// <summary>
        ///     Log a warning level security event if 'warning' level logging is enabled
        ///     and also record the stack trace associated with the event.
        /// </summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        /// <param name="throwable">
        ///     The exception to log.
        /// </param>
        void Warning(int type, string message, Exception throwable);

        /// <summary>
        ///     Allows the caller to determine if messages logged at this level
        ///     will be discarded, to avoid performing expensive processing.
        /// </summary>
        /// <returns>true, if warning level messages will be output to the log.</returns>
        bool IsWarningEnabled();

        /// <summary> Log a warning level security event if 'info' level logging is enabled.</summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        void Info(int type, string message);

        /// <summary>
        ///     Log a warning level security event if 'info' level logging is enabled
        ///     and also record the stack trace associated with the event.
        /// </summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        /// <param name="throwable">
        ///     The exception to log.
        /// </param>
        void Info(int type, string message, Exception throwable);

        /// <summary>
        ///     Allows the caller to determine if messages logged at this level
        ///     will be discarded, to avoid performing expensive processing.
        /// </summary>
        /// <returns>true, if info level messages will be output to the log.</returns>
        bool IsInfoEnabled();

        /// <summary> Log a warning level security event if 'debug' level logging is enabled.</summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        void Debug(int type, string message);

        /// <summary>
        ///     Log a warning level security event if 'debug' level logging is enabled
        ///     and also record the stack trace associated with the event.
        /// </summary>
        /// <param name="type">
        ///     The type of event.
        /// </param>
        /// <param name="message">
        ///     The message to log.
        /// </param>
        /// <param name="throwable">
        ///     The exception to log.
        /// </param>
        void Debug(int type, string message, Exception throwable);

        /// <summary>
        ///     Allows the caller to determine if messages logged at this level
        ///     will be discarded, to avoid performing expensive processing.
        /// </summary>
        /// <returns>true, if debug level messages will be output to the log.</returns>
        bool IsDebugEnabled();
    }
}
