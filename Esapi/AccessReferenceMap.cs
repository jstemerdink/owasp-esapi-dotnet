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
using System.Collections;
using System.Collections.Generic;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap" />
    /// <summary>
    ///     Reference <see cref="Owasp.Esapi.Interfaces.IAccessReferenceMap" /> implementation uses short random strings to
    ///     create a layer of indirection. Other possible implementations would use
    ///     simple integers as indirect references.
    /// </summary>
    public class AccessReferenceMap : IAccessReferenceMap
    {
        private readonly IRandomizer random = Esapi.Randomizer;

        private Dictionary<object, string> dtoi = new Dictionary<object, string>();

        private Dictionary<string, object> itod = new Dictionary<string, object>();

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public AccessReferenceMap()
        {
        }

        /// <summary>
        ///     Constructor that accepts collection of direct references.
        /// </summary>
        /// <param name="directReferences">
        ///     The collection of direct references to initialize the access reference map.
        /// </param>
        public AccessReferenceMap(ICollection directReferences)
        {
            this.Update(directReferences);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.GetDirectReferences()" />
        public ICollection GetDirectReferences()
        {
            return this.dtoi.Keys;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.GetIndirectReferences()" />
        public ICollection GetIndirectReferences()
        {
            return this.itod.Keys;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.AddDirectReference(object)" />
        public string AddDirectReference(object direct)
        {
            if (direct == null)
            {
                throw new ArgumentNullException("direct");
            }

            string indirect = this.random.GetRandomString(6, CharSetValues.Alphanumerics);
            this.itod[indirect] = direct;
            this.dtoi[direct] = indirect;
            return indirect;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.RemoveDirectReference(object)" />
        public string RemoveDirectReference(object direct)
        {
            if (direct == null)
            {
                throw new ArgumentNullException("direct");
            }

            string indirect = this.dtoi[direct];
            if (indirect != null)
            {
                this.itod.Remove(indirect);
                this.dtoi.Remove(direct);
            }
            return indirect;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.Update(ICollection)" />
        public void Update(ICollection directReferences)
        {
            if (directReferences == null)
            {
                throw new ArgumentNullException("directReferences");
            }

            // Avoid making copies / deletions, collect new records then update current
            Dictionary<object, string> dtoi_new = new Dictionary<object, string>();
            Dictionary<string, object> itod_new = new Dictionary<string, object>();

            foreach (object direct in directReferences)
            {
                // get the old indirect reference
                string indirect;

                if (!this.dtoi.TryGetValue(direct, out indirect) || indirect == null)
                {
                    // if the old reference is null, then create a new one that doesn't
                    // collide with any existing indirect references
                    do
                    {
                        indirect = this.random.GetRandomString(6, CharSetValues.Alphanumerics);
                    }
                    while (itod_new.ContainsKey(indirect));
                }

                itod_new[indirect] = direct;
                dtoi_new[direct] = indirect;
            }

            this.itod = itod_new;
            this.dtoi = dtoi_new;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.GetIndirectReference(object)" />
        public string GetIndirectReference(object directReference)
        {
            if (directReference == null)
            {
                throw new ArgumentNullException("directReference");
            }

            string indirect;
            this.dtoi.TryGetValue(directReference, out indirect);

            return indirect;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessReferenceMap.GetDirectReference(string)" />
        public object GetDirectReference(string indirectReference)
        {
            if (indirectReference == null)
            {
                throw new ArgumentNullException("indirectReference");
            }

            if (!this.itod.ContainsKey(indirectReference))
            {
                throw new AccessControlException(
                    EM.AccessReferenceMap_AccessDeniedUser,
                    EM.AccessReferenceMap_AccessDeniedLog);
            }

            return this.itod[indirectReference];
        }
    }
}
