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

using System.Collections;
using System.Security.Principal;

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The ISecurityConfiguration interface stores all configuration information
    ///     that directs the behavior of the ESAPI implementation.
    /// </summary>
    public interface ISecurityConfiguration
    {
        /// <summary>
        ///     The master password.
        /// </summary>
        string MasterPassword { get; }

        /// <summary>
        ///     The master salt.
        /// </summary>
        byte[] MasterSalt { get; }

        /// <summary>
        ///     The allowed file extensions.
        /// </summary>
        IList AllowedFileExtensions { get; }

        /// <summary>
        ///     The allowed file upload size.
        /// </summary>
        int AllowedFileUploadSize { get; }

        /// <summary>
        ///     The encryption algorithm.
        /// </summary>
        string EncryptionAlgorithm { get; }

        /// <summary>
        ///     The hasing algorithm.
        /// </summary>
        string HashAlgorithm { get; }

        /// <summary>
        ///     The character encoding.
        /// </summary>
        string CharacterEncoding { get; }

        /// <summary>
        ///     The digital signature algorithm.
        /// </summary>
        string DigitalSignatureAlgorithm { get; }

        /// <summary>
        ///     The random number generation algorithm.
        /// </summary>
        string RandomAlgorithm { get; }

        /// <summary>
        ///     The log level to use for logging.
        /// </summary>
        int LogLevel { get; }

        /// <summary>
        ///     Whether or not HTML encoding is required in the log file.
        /// </summary>
        bool LogEncodingRequired { get; }

        /// <summary>
        ///     Current user
        /// </summary>
        IPrincipal CurrentUser { get; }
    }
}
