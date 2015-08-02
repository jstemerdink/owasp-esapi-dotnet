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

using Owasp.Esapi.Interfaces;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IValidator" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.IValidator" /> interface. This implementation
    ///     keeps the validation rules in a map. It also adds the default set of validation rules defined in the reference
    ///     implementation.
    /// </summary>
    public class Validator : IValidator
    {
        private readonly Dictionary<string, IValidationRule> rules = new Dictionary<string, IValidationRule>();

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IValidator.AddRule(string, IValidationRule)" />
        public void AddRule(string name, IValidationRule rule)
        {
            // NOTE: "name" will be validated by the dictionary
            if (rule == null)
            {
                throw new ArgumentNullException("rule");
            }
            this.rules.Add(name, rule);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IValidator.GetRule(string)" />
        public IValidationRule GetRule(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            IValidationRule rule;
            this.rules.TryGetValue(name, out rule);

            return rule;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IValidator.RemoveRule(string)" />
        public void RemoveRule(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.rules.Remove(name);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IValidator.IsValid(string, string)" />
        public bool IsValid(string ruleName, string input)
        {
            if (ruleName == null)
            {
                throw new ArgumentNullException("ruleName");
            }

            return this.GetRule(ruleName).IsValid(input);
        }
    }
}
