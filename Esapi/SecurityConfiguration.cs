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
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

using Owasp.Esapi.Configuration;
using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.ISecurityConfiguration" /> interface
    ///     manages all the settings used by the ESAPI in a single place.
    /// </summary>
    /// <remarks>
    ///     You must have the relevant configuration in your config file (app.config, web.config).
    ///     See the app.config file in the EsapiTest project for the necessary elements.
    /// </remarks>
    public class SecurityConfiguration : ISecurityConfiguration
    {
        private readonly SecurityConfigurationElement _settings;

        /// <summary> Instantiates a new configuration.</summary>
        internal SecurityConfiguration(SecurityConfigurationElement settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this._settings = settings;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.MasterPassword" />
        public string MasterPassword
        {
            get
            {
                return this._settings.Encryption.MasterPassword;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.MasterSalt" />
        public byte[] MasterSalt
        {
            get
            {
                return Encoding.ASCII.GetBytes(this._settings.Encryption.MasterSalt);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.AllowedFileExtensions" />
        public IList AllowedFileExtensions
        {
            get
            {
                string[] extensions = this._settings.Application.UploadValidExtensions.Split(
                    new[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries);
                return new List<string>(extensions);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.AllowedFileUploadSize" />
        public int AllowedFileUploadSize
        {
            get
            {
                return this._settings.Application.UploadMaxSize;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.EncryptionAlgorithm" />
        public string EncryptionAlgorithm
        {
            get
            {
                return this._settings.Algorithms.Encryption;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.HashAlgorithm" />
        public string HashAlgorithm
        {
            get
            {
                return this._settings.Algorithms.Hash;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.CharacterEncoding" />
        public string CharacterEncoding
        {
            get
            {
                return this._settings.Application.CharacterEncoding;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.DigitalSignatureAlgorithm" />
        public string DigitalSignatureAlgorithm
        {
            get
            {
                return this._settings.Algorithms.DigitalSignature;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.RandomAlgorithm" />
        public string RandomAlgorithm
        {
            get
            {
                return this._settings.Algorithms.Random;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.LogLevel" />
        public int LogLevel
        {
            get
            {
                return LogLevels.ParseLogLevel(this._settings.Application.LogLevel);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.LogEncodingRequired" />
        public bool LogEncodingRequired
        {
            get
            {
                return this._settings.Application.LogEncodingRequired;
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.ISecurityConfiguration.CurrentUser" />
        public IPrincipal CurrentUser
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.User;
                }
                return Thread.CurrentPrincipal;
            }
        }
    }
}
