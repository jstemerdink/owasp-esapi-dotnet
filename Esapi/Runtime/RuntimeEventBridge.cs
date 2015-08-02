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
    ///     Base class to forward runtime events
    /// </summary>
    internal class RuntimeEventBridge : IRuntimeEventListener, IRuntimeEventPublisher
    {
        #region IRuntimeEventListener implementation

        /// <summary>
        ///     Subscribe to publisher's events
        /// </summary>
        /// <param name="publisher"></param>
        public virtual void Subscribe(IRuntimeEventPublisher publisher)
        {
            publisher.PreRequestHandlerExecute += this.OnPreRequestHandlerExecute;
            publisher.PostRequestHandlerExecute += this.OnPostRequestHandlerExecute;
        }

        /// <summary>
        ///     Disconnect from publisher's events
        /// </summary>
        /// <param name="publisher"></param>
        public virtual void Unsubscribe(IRuntimeEventPublisher publisher)
        {
            publisher.PreRequestHandlerExecute -= this.OnPreRequestHandlerExecute;
            ;
            publisher.PostRequestHandlerExecute -= this.OnPostRequestHandlerExecute;
        }

        #endregion

        #region IRuntimeEventPublisher implementation

        /// <summary>
        ///     Before request handler is executed
        /// </summary>
        public event EventHandler<RuntimeEventArgs> PreRequestHandlerExecute;

        /// <summary>
        ///     After request handler is executed
        /// </summary>
        public event EventHandler<RuntimeEventArgs> PostRequestHandlerExecute;

        #endregion

        #region Overridables

        /// <summary>
        ///     Before event is forwarded template method
        /// </summary>
        /// <param name="handler">Handler to which the event is forwarded</param>
        /// <param name="sender">Who's sending the event</param>
        /// <param name="args">Arguments</param>
        /// <returns>True if event handled (not forwarded necessary), false otherwise</returns>
        /// <remarks>Override to prevent event forwarding</remarks>
        protected virtual bool BeforeForwardEvent(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args)
        {
            return false;
        }

        /// <summary>
        ///     After event is forwarded
        /// </summary>
        /// <param name="handler">Handler to which the event is forwarded</param>
        /// <param name="sender">Who's sending the event</param>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        /// <remarks>Override to be called after the event is forwarded</remarks>
        protected virtual bool AfterForwardEvent(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args)
        {
            return false;
        }

        /// <summary>
        ///     Event forward operation failed (exception thrown)
        /// </summary>
        /// <param name="handler">Handler to which the event is forwarded</param>
        /// <param name="sender">Who's sending the event</param>
        /// <param name="args">Arguments</param>
        /// <param name="exp">Exception thrown</param>
        /// <returns>True is fault handled, false otherwise</returns>
        /// <remarks>Override to process forward exceptions</remarks>
        protected virtual bool ForwardEventFault(
            EventHandler<RuntimeEventArgs> handler,
            object sender,
            RuntimeEventArgs args,
            Exception exp)
        {
            return false;
        }

        #endregion

        #region Event connectors

        /// <summary>
        ///     Forward event
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ForwardEvent(EventHandler<RuntimeEventArgs> handler, object sender, RuntimeEventArgs args)
        {
            if (handler != null)
            {
                try
                {
                    if (this.BeforeForwardEvent(handler, sender, args))
                    {
                        return;
                    }

                    handler(sender, args);

                    if (this.AfterForwardEvent(handler, sender, args))
                    {
                    }
                }
                catch (Exception exp)
                {
                    if (!this.ForwardEventFault(handler, sender, args, exp))
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Bridge method - before handler exec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPreRequestHandlerExecute(object sender, RuntimeEventArgs args)
        {
            this.ForwardEvent(this.PreRequestHandlerExecute, sender, args);
        }

        /// <summary>
        ///     Bridge method - after handler exec
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnPostRequestHandlerExecute(object sender, RuntimeEventArgs args)
        {
            this.ForwardEvent(this.PostRequestHandlerExecute, sender, args);
        }

        #endregion
    }
}
