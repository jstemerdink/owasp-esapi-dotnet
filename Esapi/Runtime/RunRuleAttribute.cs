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

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     Request rule execution at runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RunRuleAttribute : Attribute
    {
        private Type[] _faultActions;

        private Type _ruleType;

        /// <summary>
        ///     Initialize required rule to run
        /// </summary>
        /// <param name="ruleType">Rule type</param>
        public RunRuleAttribute(Type ruleType)
            : this(ruleType, null)
        {
        }

        /// <summary>
        ///     Initialize required rule to run
        /// </summary>
        /// <param name="ruleType">Rule type</param>
        /// <param name="faultActions">Actions to run on rule failure</param>
        public RunRuleAttribute(Type ruleType, Type[] faultActions)
        {
            if (ruleType == null)
            {
                throw new ArgumentNullException();
            }
            this._ruleType = ruleType;
            this._faultActions = faultActions;
        }

        /// <summary>
        ///     Type of rule to run
        /// </summary>
        public Type Rule
        {
            get
            {
                return this._ruleType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this._ruleType = value;
            }
        }

        /// <summary>
        ///     Actions to run if the rule fails
        /// </summary>
        public Type[] FaultActions
        {
            get
            {
                return this._faultActions;
            }
            set
            {
                this._faultActions = value;
            }
        }
    }
}
