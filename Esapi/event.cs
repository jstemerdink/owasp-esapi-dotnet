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

using Owasp.Esapi.Errors;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <summary>
    ///     Security event
    /// </summary>
    internal class Event : IEquatable<Event>
    {
        private readonly string _name;

        private readonly List<DateTime> _times;

        public Event(string name)
        {
            this._name = name;
            this._times = new List<DateTime>();
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        #region IEquatable<Event> Members

        public bool Equals(Event other)
        {
            if (other == null)
            {
                return false;
            }
            return this._name == other.Name;
        }

        #endregion

        public void Increment(int maxOccurences, TimeSpan maxTimeSpan)
        {
            DateTime now = DateTime.Now;
            this._times.Add(now);

            while (this._times.Count > maxOccurences)
            {
                this._times.RemoveAt(this._times.Count - 1);
            }

            if (this._times.Count == maxOccurences)
            {
                if (now - this._times[maxOccurences - 1] < maxTimeSpan)
                {
                    throw new IntrusionException(
                        EM.IntrusionDetector_ThresholdExceeded,
                        string.Format(EM.InstrusionDetector_ThresholdExceeded1, this._name));
                }
            }
        }

        #region Object overrides

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Event);
        }

        #endregion
    }
}
