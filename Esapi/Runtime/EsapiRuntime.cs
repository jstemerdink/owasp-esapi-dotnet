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
using System.Threading;

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     ESAPI Runtime implementation
    /// </summary>
    internal class EsapiRuntime : RuntimeEventBridge, IRuntime, IDisposable
    {
        private readonly NamedObjectRepository<IAction> _actions;

        private readonly NamedObjectRepository<ICondition> _conditions;

        private readonly NamedObjectRepository<IContext> _contexts;

        private readonly ReaderWriterLockSlim _contextsLock;

        private readonly NamedObjectRepository<IRule> _rules;

        /// <summary>
        ///     Initialize runtime instance
        /// </summary>
        public EsapiRuntime()
        {
            this._actions = new NamedObjectRepository<IAction>();
            this._rules = new NamedObjectRepository<IRule>();
            this._conditions = new NamedObjectRepository<ICondition>();

            this._contextsLock = new ReaderWriterLockSlim();
            this._contexts = new NamedObjectRepository<IContext>();
        }

        /// <summary>
        ///     Disconnect from publisher's events
        /// </summary>
        /// <param name="publisher"></param>
        public override void Unsubscribe(IRuntimeEventPublisher publisher)
        {
            base.Unsubscribe(publisher);

            this._contextsLock.EnterReadLock();

            try
            {
                foreach (IContext context in this._contexts.Objects)
                {
                    IRuntimeEventListener rteListener = context as IRuntimeEventListener;
                    if (rteListener != null)
                    {
                        rteListener.Unsubscribe(this);
                    }
                }
            }
            finally
            {
                this._contextsLock.ExitReadLock();
            }
        }

        #region IEsapiRuntime implementation        

        /// <summary>
        ///     Runtime registered actions
        /// </summary>
        public IObjectRepository<string, IAction> Actions
        {
            get
            {
                return this._actions;
            }
        }

        /// <summary>
        ///     Runtime registered rules
        /// </summary>
        public IObjectRepository<string, IRule> Rules
        {
            get
            {
                return this._rules;
            }
        }

        /// <summary>
        ///     Runtime registered conditions
        /// </summary>
        public IObjectRepository<string, ICondition> Conditions
        {
            get
            {
                return this._conditions;
            }
        }

        /// <summary>
        ///     Context hierarchy
        /// </summary>
        public ICollection<IContext> Contexts
        {
            get
            {
                return this._contexts.Objects;
            }
        }

        /// <summary>
        ///     Register new context
        /// </summary>
        /// <returns></returns>
        /// <remarks>Context name is automatically generated</remarks>
        public IContext CreateContext()
        {
            return this.CreateContext(Guid.NewGuid().ToString());
        }

        /// <summary>
        ///     Register named context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContext CreateContext(string name)
        {
            this._contextsLock.EnterWriteLock();

            try
            {
                IContext prevContext;
                if (this._contexts.Lookup(name, out prevContext))
                {
                    throw new ArgumentException();
                }

                using (Context context = new Context(name))
                {
                    context.Subscribe(this);

                    this._contexts.Register(name, context);
                    return context;
                }
            }
            finally
            {
                this._contextsLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Lookup context by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContext LookupContext(string name)
        {
            this._contextsLock.EnterReadLock();

            try
            {
                IContext context;
                this._contexts.Lookup(name, out context);
                return context;
            }
            finally
            {
                this._contextsLock.ExitReadLock();
            }
        }

        /// <summary>
        ///     Register context
        /// </summary>
        /// <param name="name"></param>
        /// <param name="context"></param>
        public void RegisterContext(string name, IContext context)
        {
            this._contextsLock.EnterWriteLock();

            try
            {
                IContext prevContext;
                if (this._contexts.Lookup(name, out prevContext))
                {
                    throw new ArgumentException();
                }

                if (context is IRuntimeEventListener)
                {
                    ((IRuntimeEventListener)context).Subscribe(this);
                }
                this._contexts.Register(name, context);
            }
            finally
            {
                this._contextsLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Remove context
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IContext RemoveContext(string name)
        {
            this._contextsLock.EnterWriteLock();

            try
            {
                IContext context;
                if (this._contexts.Lookup(name, out context))
                {
                    this._contexts.Revoke(name);
                }
                return context;
            }
            finally
            {
                this._contextsLock.ExitWriteLock();
            }
        }

        #endregion

        #region IDisposable implementation

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._contextsLock.EnterReadLock();

                try
                {
                    foreach (IContext context in this._contexts.Objects)
                    {
                        IDisposable ctxDispose = context as IDisposable;
                        if (ctxDispose != null)
                        {
                            ctxDispose.Dispose();
                        }
                    }
                }
                finally
                {
                    this._contextsLock.ExitReadLock();
                    this._contextsLock.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        // Disposable types implement a finalizer.
        ~EsapiRuntime()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
