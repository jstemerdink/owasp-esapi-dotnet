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
using System.Diagnostics;
using System.Security.Principal;

using log4net;
using log4net.Config;

using Owasp.Esapi.Codecs;
using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi
{
    /// <summary>
    ///     These are fields for the logger class.
    /// </summary>
    public class LogLevels
    {
        /// <summary>
        ///     Logging is disabled.
        /// </summary>
        public static readonly int OFF = int.MaxValue;

        /// <summary>
        ///     Only fatal log messages are recorded.
        /// </summary>
        public static readonly int FATAL = 1000;

        /// <summary>
        ///     Only error-level log messages are recorded.
        /// </summary>
        public static readonly int ERROR = 800;

        /// <summary>
        ///     Only warning-level log messages are recorded.
        /// </summary>
        public static readonly int WARN = 600;

        /// <summary>
        ///     Only informational log messages are recorded.
        /// </summary>
        public static readonly int INFO = 400;

        /// <summary>
        ///     Only debug log messages are recoreded.
        /// </summary>
        public static readonly int DEBUG = 200;

        /// <summary>
        ///     All log messages are recorded.
        /// </summary>
        public static readonly int ALL = int.MinValue;

        /// <summary>
        ///     This method parses the string indiciating log level and returns the appropriate integer.
        /// </summary>
        /// <param name="level">The string indicating the log level.</param>
        /// <returns>The integer representing the log level.</returns>
        public static int ParseLogLevel(string level)
        {
            if (!string.IsNullOrEmpty(level))
            {
                if (0 == string.Compare(level, "FATAL", StringComparison.InvariantCultureIgnoreCase))
                {
                    return FATAL;
                }
                if (0 == string.Compare(level, "ERROR", StringComparison.InvariantCultureIgnoreCase))
                {
                    return ERROR;
                }
                if (0 == string.Compare(level, "WARNING", StringComparison.InvariantCultureIgnoreCase))
                {
                    return WARN;
                }
                if (0 == string.Compare(level, "INFO", StringComparison.InvariantCultureIgnoreCase))
                {
                    return INFO;
                }
                if (0 == string.Compare(level, "DEBUG", StringComparison.InvariantCultureIgnoreCase))
                {
                    return DEBUG;
                }
                if (0 == string.Compare(level, "OFF", StringComparison.InvariantCultureIgnoreCase))
                {
                    return OFF;
                }
            }
            return ALL;
        }
    }

    /// <summary>
    ///     This class contains the keys for the different event types that can be passed to the logger.
    /// </summary>
    public class LogEventTypes
    {
        /// <summary>
        ///     Used for security events.
        /// </summary>
        public static readonly int SECURITY = 0;

        /// <summary>
        ///     Used for usability events.
        /// </summary>
        public static readonly int USABILITY = 1;

        /// <summary>
        ///     Used for performance events.
        /// </summary>
        public static readonly int PERFORMANCE = 2;

        /// <summary>
        ///     Used for functionality events.
        /// </summary>
        public static readonly int FUNCTIONALITY = 3;

        internal static string GetType(int type)
        {
            switch (type)
            {
                case 0:
                    return "SECURITY";
                case 1:
                    return "USABILITY";
                case 2:
                    return "PERFORMANCE";
                case 3:
                    return "FUNCTIONALITY";
            }
            return "UNDEFINED";
        }
    }

    /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.ILogger" /> interface. This implementation uses
    ///     the Log4Net logging package,
    ///     and marks each log message with the currently logged in user and the word "SECURITY" for security related events.
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>The Log4Net logger.</summary>
        private readonly ILog logger;

        /// <summary>The application name.</summary>
        private string applicationName;

        /// <summary>The module name.</summary>
        private string moduleName;

        static Logger()
        {
            XmlConfigurator.Configure();
        }

        /// <summary>
        ///     The constructor, which is hidden (private) and accessed through Esapi class.
        /// </summary>
        public Logger(string className)
        {
            this.logger = LogManager.GetLogger(className);
            this.Level = Esapi.SecurityConfiguration.LogLevel;
            if (this.Level == LogLevels.FATAL)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Fatal;
            }
            else if (this.Level == LogLevels.ERROR)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Error;
            }
            else if (this.Level == LogLevels.WARN)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Warn;
            }
            else if (this.Level == LogLevels.INFO)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Info;
            }
            else if (this.Level == LogLevels.DEBUG)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Debug;
            }
            else if (this.Level == LogLevels.OFF)
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.Off;
            }
            else
            {
                LogManager.GetRepository().Threshold = log4net.Core.Level.All;
            }
        }

        /// <summary>
        ///     The constructor, which is hidden (private) and accessed through Esapi class.
        /// </summary>
        /// <param name="applicationName">
        ///     The application name.
        /// </param>
        /// <param name="moduleName">
        ///     The module name.
        /// </param>
        private Logger(string applicationName, string moduleName)
        {
            this.applicationName = applicationName;
            this.moduleName = moduleName;
        }

        /// <summary>
        ///     Log the message after optionally encoding any special characters that might inject into an HTML
        ///     based log viewer. This method accepts an exception.
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
        private string GetLogMessage(int type, string message, Exception throwable)
        {
            IPrincipal currentUser = Esapi.SecurityConfiguration.CurrentUser;

            // Ensure no CRLF injection into logs for forging records
            string clean = !string.IsNullOrEmpty(message) ? message.Replace('\n', '_').Replace('\r', '_') : message;

            // HTML encode log message if it will be viewed in a web browser
            if (Esapi.SecurityConfiguration.LogEncodingRequired)
            {
                clean = Esapi.Encoder.Encode(BuiltinCodecs.Html, message);
                if (!message.Equals(clean))
                {
                    clean += " (Encoded)";
                }
            }

            // Add a printable stack trace
            if (throwable != null)
            {
                string fqn = throwable.GetType().FullName;
                int index = fqn.LastIndexOf('.');
                if (index > 0)
                {
                    fqn = fqn.Substring(index + 1);
                }
                StackTrace st = new StackTrace(throwable, true);

                StackFrame[] frames = st.GetFrames();
                if (frames != null)
                {
                    StackFrame frame = frames[0];
                    clean += ("\n    " + throwable.Message + " - " + fqn + " @ " + "(" + frame.GetFileName() + ":"
                              + frame.GetFileLineNumber() + ")");
                }
            }

            string msg;

            if (currentUser != null && currentUser.Identity != null)
            {
                msg = LogEventTypes.GetType(type) + ": " + currentUser.Identity.Name + ": " + clean;
            }
            else
            {
                msg = LogEventTypes.GetType(type) + ": " + clean;
            }

            return msg;
        }

        #region ILogger Members

        private int level;

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Level" />
        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Fatal(int, string)" />
        public void Fatal(int type, string message)
        {
            if (this.logger.IsFatalEnabled)
            {
                this.logger.Fatal(this.GetLogMessage(type, message, null));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Fatal(int, string, Exception)" />
        public void Fatal(int type, string message, Exception exception)
        {
            if (this.logger.IsFatalEnabled)
            {
                this.logger.Fatal(this.GetLogMessage(type, message, exception));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.IsFatalEnabled()" />
        public bool IsFatalEnabled()
        {
            return (this.logger.IsFatalEnabled);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Error(int, string)" />
        public void Error(int type, string message)
        {
            if (this.logger.IsErrorEnabled)
            {
                this.logger.Error(this.GetLogMessage(type, message, null));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Error(int, string, Exception)" />
        public void Error(int type, string message, Exception throwable)
        {
            if (this.logger.IsErrorEnabled)
            {
                this.logger.Error(this.GetLogMessage(type, message, throwable));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.IsErrorEnabled()" />
        public bool IsErrorEnabled()
        {
            return (this.logger.IsErrorEnabled);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Warning(int, string)" />
        public void Warning(int type, string message)
        {
            if (this.logger.IsWarnEnabled)
            {
                this.logger.Warn(this.GetLogMessage(type, message, null));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Warning(int, string, Exception)" />
        public void Warning(int type, string message, Exception throwable)
        {
            if (this.logger.IsWarnEnabled)
            {
                this.logger.Warn(this.GetLogMessage(type, message, throwable));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.IsWarningEnabled()" />
        public bool IsWarningEnabled()
        {
            return (this.logger.IsWarnEnabled);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Info(int, string)" />
        public void Info(int type, string message)
        {
            if (this.logger.IsInfoEnabled)
            {
                this.logger.Info(this.GetLogMessage(type, message, null));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Info(int, string, Exception)" />
        public void Info(int type, string message, Exception throwable)
        {
            if (this.logger.IsInfoEnabled)
            {
                this.logger.Info(this.GetLogMessage(type, message, throwable));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.IsInfoEnabled()" />
        public bool IsInfoEnabled()
        {
            return this.logger.IsInfoEnabled;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Debug(int, string)" />
        public void Debug(int type, string message)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug(this.GetLogMessage(type, message, null));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.Debug(int, string, Exception)" />
        public void Debug(int type, string message, Exception throwable)
        {
            if (this.logger.IsDebugEnabled)
            {
                this.logger.Debug(this.GetLogMessage(type, message, throwable));
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ILogger.IsDebugEnabled()" />
        public bool IsDebugEnabled()
        {
            return this.logger.IsDebugEnabled;
        }

        #endregion
    }
}
