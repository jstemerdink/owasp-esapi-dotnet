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
using System.Diagnostics;

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     Context implementation
    /// </summary>
    internal class Context : RuntimeEventBridge, IContext, IDisposable
    {
        private readonly List<IContextCondition> _conditions;

        private readonly string _name;

        private readonly List<IContextRule> _rules;

        private readonly NamedObjectRepository<IContext> _subcontexts;

        internal Context(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException();
            }

            this._name = name;

            this._conditions = new List<IContextCondition>();
            this._rules = new List<IContextRule>();
            this._subcontexts = new NamedObjectRepository<IContext>();
        }

        public override void Unsubscribe(IRuntimeEventPublisher publisher)
        {
            base.Unsubscribe(publisher);

            foreach (ContextRule rule in this._rules)
            {
                rule.Unsubscribe(this);
            }
            foreach (IContext subcontext in this._subcontexts.Objects)
            {
                IRuntimeEventListener rteListener = subcontext as IRuntimeEventListener;
                if (rteListener != null)
                {
                    rteListener.Unsubscribe(this);
                }
            }
        }

        /// <summary>
        ///     Check if the context is matched
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool IsMatch(RuntimeEventArgs args)
        {
            bool isMatch = true;

            // Check context match cache first
            if (args.MatchCache.TryGetValue(this, out isMatch))
            {
                return isMatch;
            }

            // Initialize condition arguments
            isMatch = true;
            ConditionArgs conditionArgs = new ConditionArgs { RuntimeArgs = args };

            // Evaluate each condition
            foreach (IContextCondition contextCondition in this._conditions)
            {
                bool result = true;

                // Check condition eval cache first
                if (!args.EvalCache.TryGetValue(contextCondition.Condition, out result))
                {
                    // Eval
                    result = (contextCondition.Condition.Evaluate(conditionArgs) == contextCondition.Result);
                    args.EvalCache.SetValue(contextCondition.Condition, result);
                }

                // Shortcut match if false
                if (!result)
                {
                    isMatch = false;
                    break;
                }
            }

            // Cache
            args.MatchCache.SetValue(this, isMatch);

            // Return
            return isMatch;
        }

        #region EventBridge

        protected override bool ForwardEventFault(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args,
            Exception exp)
        {
            IContext top = args.PopContext();
            Debug.Assert(top == this);

            return base.ForwardEventFault(handler, sender, args, exp);
        }

        protected override bool AfterForwardEvent(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args)
        {
            IContext top = args.PopContext();
            Debug.Assert(top == this);

            return base.AfterForwardEvent(handler, sender, args);
        }

        protected override bool BeforeForwardEvent(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args)
        {
            bool shouldContinue = !this.IsMatch(args);
            if (!shouldContinue)
            {
                args.PushContext(this);
            }
            return shouldContinue;
        }

        #endregion EventBridge

        #region IContext implementation

        /// <summary>
        ///     Get context name
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        /// <summary>
        ///     Get context conditions
        /// </summary>
        public ICollection<IContextCondition> MatchConditions
        {
            get
            {
                return this._conditions;
            }
        }

        /// <summary>
        ///     Get context rules
        /// </summary>
        public ICollection<IContextRule> ExecuteRules
        {
            get
            {
                return this._rules;
            }
        }

        public ICollection<IContext> SubContexts
        {
            get
            {
                return this._subcontexts.Objects;
            }
        }

        /// <summary>
        ///     Add condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="value">Value to match</param>
        /// <returns></returns>
        public IContextCondition BindCondition(ICondition condition, bool value)
        {
            ContextCondition contextCondition = new ContextCondition(condition, value);
            this._conditions.Add(contextCondition);

            return contextCondition;
        }

        /// <summary>
        ///     Bind rule
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public IContextRule BindRule(IRule rule)
        {
            ContextRule contextRule = new ContextRule(rule);
            contextRule.Subscribe(this);

            this._rules.Add(contextRule);
            return contextRule;
        }

        /// <summary>
        ///     Register context
        /// </summary>
        /// <returns>New subcontext</returns>
        /// <remarks>Context name is automatically generated</remarks>
        public IContext CreateSubContext()
        {
            return this.CreateSubContext(Guid.NewGuid().ToString());
        }

        /// <summary>
        ///     Register named context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContext CreateSubContext(string name)
        {
            IContext prevContext;
            if (this._subcontexts.Lookup(name, out prevContext))
            {
                throw new ArgumentException();
            }

            using (Context context = new Context(name))
            {
                context.Subscribe(this);

                this._subcontexts.Register(name, context);
                return context;
            }
        }

        /// <summary>
        ///     Lookup subcontext by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContext LookupSubContext(string name)
        {
            IContext context;
            this._subcontexts.Lookup(name, out context);
            return context;
        }

        /// <summary>
        ///     Register subcontext
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        public void RegisterSubContext(string name, IContext context)
        {
            this._subcontexts.Register(name, context);
        }

        #endregion

        #region IDisposable implementation

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (ContextRule rule in this._rules)
                {
                    rule.Dispose();
                }
                foreach (IContext subcontext in this._subcontexts.Objects)
                {
                    IDisposable disp = subcontext as IDisposable;

                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        // Disposable types implement a finalizer.
        ~Context()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
