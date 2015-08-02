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
using System.Text.RegularExpressions;

using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi.ValidationRules
{
    [ValidationRule(BuiltinValidationRules.String, AutoLoad = false)]
    public class StringValidationRule : IValidationRule
    {
        private readonly List<Regex> _blacklist;

        private readonly List<Regex> _whitelist;

        private bool _allowNullOrEmpty;

        private int _maxLength = int.MaxValue;

        private int _minLength;

        /// <summary>
        ///     Initialize string validation rule
        /// </summary>
        public StringValidationRule()
        {
            this._whitelist = new List<Regex>();
            this._blacklist = new List<Regex>();
        }

        /// <summary>
        ///     Allow null or empty values
        /// </summary>
        public bool AllowNullOrEmpty
        {
            get
            {
                return this._allowNullOrEmpty;
            }
            set
            {
                this._allowNullOrEmpty = value;
            }
        }

        /// <summary>
        ///     Minimum length value
        /// </summary>
        public int MinLength
        {
            get
            {
                return this._minLength;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(EM.InvalidArgument);
                }
                this._minLength = value;
            }
        }

        /// <summary>
        ///     Maximum length value
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this._maxLength;
            }
            set
            {
                this._maxLength = value;
            }
        }

        #region IValidationRule Members

        /// <summary>
        ///     Validate string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsValid(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return this._allowNullOrEmpty;
            }

            // Check length
            if (input.Length < this._minLength || input.Length > this._maxLength)
            {
                return false;
            }

            // Check whitelist patterns
            foreach (Regex r in this._whitelist)
            {
                if (!r.IsMatch(input))
                {
                    return false;
                }
            }

            // Check blacklist patterns
            foreach (Regex r in this._blacklist)
            {
                if (r.IsMatch(input))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        /// <summary>
        ///     Add pattern to whitelist
        /// </summary>
        /// <param name="pattern">String pattern</param>
        public void AddWhitelistPattern(string pattern)
        {
            try
            {
                this._whitelist.Add(new Regex(pattern));
            }
            catch (Exception exp)
            {
                throw new ArgumentException(EM.InvalidArgument, exp);
            }
        }

        /// <summary>
        ///     Add pattern to blacklist
        /// </summary>
        /// <param name="pattern">String pattern</param>
        public void AddBlacklistPattern(string pattern)
        {
            try
            {
                this._blacklist.Add(new Regex(pattern));
            }
            catch (Exception exp)
            {
                throw new ArgumentException(EM.InvalidArgument, exp);
            }
        }
    }
}
