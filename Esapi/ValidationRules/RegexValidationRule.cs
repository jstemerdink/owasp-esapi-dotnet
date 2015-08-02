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

using System.Text.RegularExpressions;

using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi.ValidationRules
{
    /// <summary>
    ///     This class is for validating that text is valid according to a given regular expression pattern.
    /// </summary>
    [ValidationRule(BuiltinValidationRules.Regex, AutoLoad = false)]
    public class RegexValidationRule : IValidationRule
    {
        private readonly Regex _regex;

        /// <summary>
        ///     Constructor that accepts regular expression.
        /// </summary>
        /// <param name="regex">The regular expression to validate against.</param>
        public RegexValidationRule(string regex)
        {
            this._regex = new Regex(regex);
        }

        #region IValidationRule Members

        /// <summary>
        ///     Checks whether the input is a valid against the specified regular expression.
        /// </summary>
        /// <param name="input">The input to valdiate.</param>
        /// <returns>True, if the input is valid. False, otherwise.</returns>
        public bool IsValid(string input)
        {
            if (input == null)
            {
                return false;
            }

            return this._regex.IsMatch(input, 0);
        }

        #endregion
    }
}
