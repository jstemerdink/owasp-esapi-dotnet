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
using System.Configuration;

using Owasp.Esapi.Configuration;

namespace Owasp.Esapi
{
    /// <summary>
    ///     Addin builder
    /// </summary>
    /// <typeparam name="TAddin"></typeparam>
    internal class AddinBuilder<TAddin>
        where TAddin : class
    {
        /// <summary>
        ///     Build addin instance
        /// </summary>
        /// <param name="configuration">Instance configuratio</param>
        /// <returns></returns>
        public static TAddin MakeInstance(AddinElement configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            // Get type
            Type typeInstance = Type.GetType(configuration.Type, true);

            // Create properties
            Dictionary<string, object> properties = null;
            if (configuration.PropertyValues != null && configuration.PropertyValues.Count > 0)
            {
                properties = new Dictionary<string, object>();

                foreach (KeyValueConfigurationElement key in configuration.PropertyValues)
                {
                    properties[key.Key] = key.Value;
                }
            }

            // Construct
            return ObjectBuilder.Build<TAddin>(typeInstance, properties);
        }
    }
}
