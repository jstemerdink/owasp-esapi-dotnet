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
using System.Text;

namespace Owasp.Esapi
{
    /// <summary>
    ///     The Threshold class is used to represent the amount of events that can be allowed, and in
    ///     what timeframe they are allowed.
    /// </summary>
    public class Threshold
    {
        /// <summary>
        ///     The list of actions associated with the threshold/
        /// </summary>
        public readonly IList<string> Actions;

        /// <summary>
        ///     The name of the event.
        /// </summary>
        public readonly string Event;

        /// <summary>
        ///     The number of occurences.
        /// </summary>
        public readonly int MaxOccurences;

        /// <summary>
        ///     The interval allowed between events.
        /// </summary>
        public readonly TimeSpan MaxTimeSpan;

        /// <summary>
        ///     Constructor for Threshold
        /// </summary>
        /// <param name="name">
        ///     Event name.
        /// </param>
        /// <param name="maxOccurences">
        ///     Count of events allowed.
        /// </param>
        /// <param name="maxTimeSpan">
        ///     Interval between events allowed.
        /// </param>
        /// <param name="actions">
        ///     Actions associated with threshold.
        /// </param>
        public Threshold(string name, int maxOccurences, long maxTimeSpan, IEnumerable<string> actions)
        {
            this.Event = name;
            this.MaxOccurences = maxOccurences;
            this.MaxTimeSpan = TimeSpan.FromSeconds(maxTimeSpan);

            this.Actions = new List<string>();

            // Add actions
            if (actions != null)
            {
                foreach (string action in actions)
                {
                    string actionName = (action != null ? action.Trim() : action);
                    if (string.IsNullOrEmpty(actionName))
                    {
                        continue;
                    }

                    this.Actions.Add(actionName);
                }
            }
        }

        /// <summary>
        ///     Returns string representation of threshold.
        /// </summary>
        /// <returns>String representation of threshold.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(
                "Threshold: {0} - {1} in {2} seconds results in ",
                this.Event,
                this.MaxOccurences,
                this.MaxTimeSpan);

            for (int i = 0; i < this.Actions.Count; ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(this.Actions[i]);
            }

            return sb.ToString();
        }
    }
}
