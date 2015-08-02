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
    ///     Action attribute
    /// </summary>
    /// <remarks>
    ///     Marks as class as an action; the class has to implement IAction
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ActionAttribute : AddinAttribute
    {
        /// <summary>
        ///     Initialize action attribute
        /// </summary>
        /// <param name="name">Action unique name</param>
        public ActionAttribute(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    ///     Rule attribute
    /// </summary>
    /// <remarks>
    ///     Marks as class as a rule; the class has to implement IRule
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class RuleAttribute : AddinAttribute
    {
        /// <summary>
        ///     Initialize rule attribute
        /// </summary>
        /// <param name="name">Rule unique name</param>
        public RuleAttribute(string name)
            : base(name)
        {
        }
    }

    /// <summary>
    ///     Condition attribute
    /// </summary>
    /// <remarks>
    ///     Marks as class as a condition ; the class has to implement IRule
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ConditionAttribute : AddinAttribute
    {
        /// <summary>
        ///     Initialize condition attribute
        /// </summary>
        /// <param name="name">Condition unique name</param>
        public ConditionAttribute(string name)
            : base(name)
        {
        }
    }
}
