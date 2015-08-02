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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IValidator interface defines a set of methods for validating untrusted input.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        ///     Checks whether input is valid according to a given rule. The rule is determined by passing a rule
        ///     name key, which is used to identify a particular ValidationRule object.
        /// </summary>
        /// <param name="rule">The rule name key to use for validation.</param>
        /// <param name="input">The input to validate.</param>
        /// <returns>True, if the data is valid. False, otherwise.</returns>
        bool IsValid(string rule, string input);

        /// <summary>
        ///     Adds a rule object with the associated rule name key. This rule can be used to
        ///     validate data later using the <see cref="Owasp.Esapi.Interfaces.IValidator.IsValid(string, string)" /> method.
        /// </summary>
        /// <param name="name">The rule name key to use for the new rule.</param>
        /// <param name="rule">
        ///     The rule object, which implements <see cref="Owasp.Esapi.Interfaces.IValidationRule" />
        /// </param>
        void AddRule(string name, IValidationRule rule);

        /// <summary>
        ///     Returns the rule object with the specified key.
        /// </summary>
        /// <param name="name">The rule name key to lookuip.</param>
        /// <returns>
        ///     The <see cref="Owasp.Esapi.Interfaces.IValidationRule" /> object associated witht the rule name
        ///     key
        /// </returns>
        IValidationRule GetRule(string name);

        /// <summary>
        ///     Removes the rule object with the specified key.
        /// </summary>
        /// <param name="name">The rule name key for the rule to remove</param>
        void RemoveRule(string name);
    }
}
