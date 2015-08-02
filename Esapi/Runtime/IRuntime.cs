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

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     Runtime interface
    /// </summary>
    public interface IRuntime
    {
        /// <summary>
        ///     Runtime named rules
        /// </summary>
        IObjectRepository<string, IRule> Rules { get; }

        /// <summary>
        ///     Runtime named conditions
        /// </summary>
        IObjectRepository<string, ICondition> Conditions { get; }

        /// <summary>
        ///     Runtime named actions
        /// </summary>
        IObjectRepository<string, IAction> Actions { get; }

        /// <summary>
        ///     Context hierarchy
        /// </summary>
        ICollection<IContext> Contexts { get; }

        /// <summary>
        ///     Create context
        /// </summary>
        /// <returns></returns>
        /// <remarks>Name is automatically generated</remarks>
        IContext CreateContext();

        /// <summary>
        ///     Create named context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IContext CreateContext(string name);

        /// <summary>
        ///     Lookup context by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IContext LookupContext(string name);

        /// <summary>
        ///     Register context
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        void RegisterContext(string name, IContext context);

        /// <summary>
        ///     Remove context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IContext RemoveContext(string name);
    }

    /// <summary>
    ///     Runtime event publisher
    /// </summary>
    public interface IRuntimeEventPublisher
    {
        /// <summary>
        ///     Before request handler execution
        /// </summary>
        event EventHandler<RuntimeEventArgs> PreRequestHandlerExecute;

        /// <summary>
        ///     After request handler execution
        /// </summary>
        event EventHandler<RuntimeEventArgs> PostRequestHandlerExecute;
    }

    /// <summary>
    ///     Runtime event listener interface
    /// </summary>
    public interface IRuntimeEventListener
    {
        /// <summary>
        ///     Subscribe to publisher's events
        /// </summary>
        /// <param name="publisher"></param>
        void Subscribe(IRuntimeEventPublisher publisher);

        /// <summary>
        ///     Disconnect from publisher's events
        /// </summary>
        /// <param name="publisher"></param>
        void Unsubscribe(IRuntimeEventPublisher publisher);
    }
}
