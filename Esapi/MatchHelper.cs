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

using System.Text;
using System.Text.RegularExpressions;

namespace Owasp.Esapi
{
    /// <summary>
    ///     Match helper
    /// </summary>
    internal class MatchHelper
    {
        /// <summary>
        ///     Convert wildcard match string to a regex
        /// </summary>
        /// <param name="wildcardMatch">Wildcard string</param>
        /// <returns></returns>
        internal static Regex WildcardToRegex(string wildcardMatch)
        {
            StringBuilder sbRegex = new StringBuilder();
            sbRegex.Append("^");

            if (!string.IsNullOrEmpty(wildcardMatch))
            {
                foreach (char w in wildcardMatch)
                {
                    if (w == '*')
                    {
                        sbRegex.Append(".*");
                        continue;
                    }
                    if (w == '?')
                    {
                        sbRegex.Append(".");
                        continue;
                    }
                    sbRegex.Append(Regex.Escape(w.ToString()));
                }
            }

            sbRegex.Append("$");

            return new Regex(sbRegex.ToString());
        }
    }
}
