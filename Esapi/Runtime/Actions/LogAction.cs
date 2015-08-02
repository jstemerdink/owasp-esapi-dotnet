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

using Owasp.Esapi.Errors;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi.Runtime.Actions
{
    /// <summary>
    ///     Log threshold exceeded action
    /// </summary>
    [Action(BuiltinActions.Log)]
    public class LogAction : IAction
    {
        #region IAction Members

        /// <summary>
        ///     Execute action
        /// </summary>
        /// <param name="args">Arguments</param>
        public void Execute(ActionArgs args)
        {
            if (args == null)
            {
                return;
            }

            IntrusionException intrusionException = args.FaultException as IntrusionException;
            if (intrusionException != null)
            {
                Esapi.Logger.Fatal(LogEventTypes.SECURITY, intrusionException.LogMessage);
            }
        }

        #endregion
    }
}
