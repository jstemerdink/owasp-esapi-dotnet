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
using System.Web;

using Owasp.Esapi.Runtime.Conditions;

namespace Owasp.Esapi.Runtime
{
    /// <summary>
    ///     ESAPI runtime module
    /// </summary>
    public class EsapiRuntimeModule : IHttpModule, IRuntimeEventPublisher, IDisposable
    {
        private readonly object _handlersLock;

        private readonly HashSet<Type> _handlerTypes;

        private readonly EsapiRuntime _runtime;

        public EsapiRuntimeModule()
        {
            this._runtime = new EsapiRuntime();

            this._handlersLock = new object();
            this._handlerTypes = new HashSet<Type>();
        }

        /// <summary>
        ///     Map request handler to context
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPostMapRequestHandler(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            IHttpHandler handler = context.CurrentHandler;

            if (handler != null)
            {
                lock (this._handlersLock)
                {
                    // Get code behind type
                    Type handlerType = handler.GetType();

                    // If handler not known map to context
                    if (!this._handlerTypes.Contains(handlerType))
                    {
                        this.MapHandlerContext(handlerType);
                        this._handlerTypes.Add(handlerType);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this.PreRequestHandlerExecute != null)
            {
                this.PreRequestHandlerExecute(this, new RuntimeEventArgs());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (this.PostRequestHandlerExecute != null)
            {
                this.PostRequestHandlerExecute(this, new RuntimeEventArgs());
            }
        }

        #region Public

        /// <summary>
        ///     Get current module instance (if registered)
        /// </summary>
        public EsapiRuntimeModule Current
        {
            get
            {
                EsapiRuntimeModule instance = null;

                // Lookup module in the current running application
                HttpApplication currentApp = HttpContext.Current != null
                                                 ? HttpContext.Current.ApplicationInstance
                                                 : null;
                if (currentApp != null)
                {
                    HttpModuleCollection modules = currentApp.Modules;

                    // Lookup first instance
                    foreach (string key in modules.Keys)
                    {
                        if (null != (instance = (modules[key] as EsapiRuntimeModule)))
                        {
                            break;
                        }
                    }
                }

                return instance;
            }
        }

        /// <summary>
        ///     Get runtime instance
        /// </summary>
        public IRuntime RuntimeInstance
        {
            get
            {
                return this._runtime;
            }
        }

        #endregion

        #region Context mapping 

        /// <summary>
        ///     Mapp application to context
        /// </summary>
        /// <param name="applicationType"></param>
        private void MapApplicationContext(Type applicationType)
        {
            Debug.Assert(applicationType != null);

            object[] runRules = applicationType.GetCustomAttributes(typeof(RunRuleAttribute), true);
            if (runRules != null && runRules.Length > 0)
            {
                // Create new context
                IContext appContext = this._runtime.CreateContext();
                appContext.BindCondition(new ValueBoundCondition(true), true);

                // Add rules to context
                foreach (RunRuleAttribute runRule in runRules)
                {
                    IContextRule rule = appContext.BindRule(ObjectBuilder.Build<IRule>(runRule.Rule));

                    // Add actions
                    if (runRule.FaultActions != null && runRule.FaultActions.Length > 0)
                    {
                        foreach (Type action in runRule.FaultActions)
                        {
                            rule.FaultActions.Add(ObjectBuilder.Build<IAction>(action));
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Map handler to context
        /// </summary>
        /// <param name="handlerTpe"></param>
        private void MapHandlerContext(Type handlerType)
        {
            Debug.Assert(handlerType != null);

            object[] runRules = handlerType.GetCustomAttributes(typeof(RunRuleAttribute), true);
            if (runRules != null && runRules.Length > 0)
            {
                // Create new context
                IContext handlerContext = this._runtime.CreateContext();
                handlerContext.BindCondition(new HandlerCondition(handlerType), true);

                // Add rules to context
                foreach (RunRuleAttribute runRule in runRules)
                {
                    IContextRule rule = handlerContext.BindRule(ObjectBuilder.Build<IRule>(runRule.Rule));

                    // Add actions
                    if (runRule.FaultActions != null && runRule.FaultActions.Length > 0)
                    {
                        foreach (Type action in runRule.FaultActions)
                        {
                            rule.FaultActions.Add(ObjectBuilder.Build<IAction>(action));
                        }
                    }
                }
            }
        }

        #endregion

        #region IRuntimeEventPublisher Members

        public event EventHandler<RuntimeEventArgs> PreRequestHandlerExecute;

        public event EventHandler<RuntimeEventArgs> PostRequestHandlerExecute;

        #endregion

        #region IHttpModule Members

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._runtime.Unsubscribe(this);
                this._runtime.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        // Disposable types implement a finalizer.
        ~EsapiRuntimeModule()
        {
            this.Dispose(false);
        }

        /// <summary>
        ///     Register for events
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
            context.PreRequestHandlerExecute += this.OnPreRequestHandlerExecute;
            context.PostMapRequestHandler += this.OnPostMapRequestHandler;

            // Connect runtime
            this._runtime.Subscribe(this);

            // Map application context
            this.MapApplicationContext(context.GetType());
        }

        #endregion
    }
}
