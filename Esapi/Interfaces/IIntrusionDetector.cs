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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IIntrusionDetector interface is intended to track security relevant events and identify attack behavior.
    /// </summary>
    public interface IIntrusionDetector
    {
        /// <summary>
        ///     The intrusion detection quota for a particular event.
        /// </summary>
        /// <param name="threshold">
        ///     The quote for a particular event name.
        /// </param>
        void AddThreshold(Threshold threshold);

        /// <summary>
        ///     Remove event threshold
        /// </summary>
        /// <param name="eventName"></param>
        bool RemoveThreshold(string eventName);

        /// <summary>
        ///     Adds the exception to the IntrusionDetector.
        /// </summary>
        /// <param name="exception">
        ///     The exception to add.
        /// </param>
        void AddException(Exception exception);

        /// <summary>
        ///     Adds the event to the IntrusionDetector.
        /// </summary>
        /// <param name="eventName">
        ///     The event to add.
        /// </param>
        void AddEvent(string eventName);
    }
}
