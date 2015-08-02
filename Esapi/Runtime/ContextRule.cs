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
    ///     Context rule implementation
    /// </summary>
    internal class ContextRule : RuntimeEventBridge, IContextRule, IDisposable
    {
        private readonly List<IAction> _faultActions;

        private readonly IRule _rule;

        /// <summary>
        ///     Initialize context rule
        /// </summary>
        /// <param name="rule">
        ///     A <see cref="IRule" />
        /// </param>
        internal ContextRule(IRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException();
            }
            this._rule = rule;
            this._faultActions = new List<IAction>();

            // Subscribe rule to events
            this._rule.Subscribe(this);
        }

        /// <summary>
        ///     Unsubscribe from publisher's events
        /// </summary>
        /// <param name="publisher">
        ///     A <see cref="IRuntimeEventPublisher" />
        /// </param>
        public override void Unsubscribe(IRuntimeEventPublisher publisher)
        {
            base.Unsubscribe(publisher);
            this._rule.Unsubscribe(this);
        }

        /// <summary>
        ///     Handle rule execution fault
        /// </summary>
        /// <param name="handler">
        ///     A <see cref="EventHandler{RuntimeEventArgs}" />
        /// </param>
        /// <param name="sender">
        ///     A <see cref="System.Object" />
        /// </param>
        /// <param name="args">
        ///     A <see cref="RuntimeEventArgs" />
        /// </param>
        /// <param name="exp">
        ///     A <see cref="Exception" />
        /// </param>
        /// <returns>
        ///     A <see cref="System.Boolean" />
        /// </returns>
        protected override bool ForwardEventFault(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args,
            Exception exp)
        {
            // Init action args
            ActionArgs actionArgs = new ActionArgs
                                        {
                                            FaultingRule = this._rule,
                                            FaultException = exp,
                                            RuntimeArgs = args
                                        };

            try
            {
                // Run each action
                foreach (IAction action in this._faultActions)
                {
                    action.Execute(actionArgs);
                }
            }
            catch (Exception)
            {
                // Nothing to do anymore, throw 
                throw;
            }

            return true;
        }

        #region IContextRule implementation

        public IRule Rule
        {
            get
            {
                return this._rule;
            }
        }

        public ICollection<IAction> FaultActions
        {
            get
            {
                return this._faultActions;
            }
        }

        #endregion

        #region IDisposable implementation

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._rule.Unsubscribe(this);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        // Disposable types implement a finalizer.
        ~ContextRule()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
