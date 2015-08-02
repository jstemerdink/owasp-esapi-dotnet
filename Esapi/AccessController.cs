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
using System.Security.Principal;

using Owasp.Esapi.Errors;
using Owasp.Esapi.Interfaces;

using EM = Owasp.Esapi.Resources.Errors;

namespace Owasp.Esapi
{
    /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessController" />
    /// <summary>
    ///     Reference implementation of the <see cref="Owasp.Esapi.Interfaces.IAccessController" /> interface. It simply
    ///     stores the access control rules in nested collections.
    /// </summary>
    public class AccessController : IAccessController
    {
        private readonly Dictionary<object, Dictionary<object, ArrayList>> resourceToSubjectsMap;

        /// <summary>
        ///     Default constructor.
        /// </summary>
        public AccessController()
        {
            this.resourceToSubjectsMap = new Dictionary<object, Dictionary<object, ArrayList>>();
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessController.IsAuthorized(object, object)" />
        public bool IsAuthorized(object action, object resource)
        {
            IPrincipal currentUser = Esapi.SecurityConfiguration.CurrentUser;

            if (currentUser == null || currentUser.Identity == null)
            {
                throw new EnterpriseSecurityException(EM.AccessControl_NoCurrentUser, EM.AccessControl_NoCurrentUser);
            }

            return this.IsAuthorized(currentUser.Identity.Name, action, resource);
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessController.IsAuthorized(object, object, object)" />
        public bool IsAuthorized(object subject, object action, object resource)
        {
            if (subject == null || action == null || resource == null)
            {
                throw new ArgumentNullException();
            }

            Dictionary<object, ArrayList> subjects;

            if (this.resourceToSubjectsMap.TryGetValue(resource, out subjects))
            {
                ArrayList actions;

                if (subjects.TryGetValue(subject, out actions))
                {
                    return actions.Contains(action);
                }
            }

            return false;
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessController.AddRule(object, object, object)" />
        public void AddRule(object subject, object action, object resource)
        {
            if (subject == null || action == null || resource == null)
            {
                throw new ArgumentNullException();
            }

            Dictionary<object, ArrayList> subjects;
            if (!this.resourceToSubjectsMap.TryGetValue(resource, out subjects))
            {
                subjects = new Dictionary<object, ArrayList>();
                this.resourceToSubjectsMap[resource] = subjects;
            }

            ArrayList actions;
            if (!subjects.TryGetValue(subject, out actions))
            {
                actions = new ArrayList();
                subjects[subject] = actions;
            }

            if (!actions.Contains(action))
            {
                actions.Add(action);
            }
            else
            {
                throw new EnterpriseSecurityException(
                    EM.AcessControl_AddDuplicateRule,
                    EM.AcessControl_AddDuplicateRule);
            }
        }

        /// <inheritdoc cref="Owasp.Esapi.Interfaces.IAccessController.RemoveRule(object, object, object)" />
        public void RemoveRule(object subject, object action, object resource)
        {
            if (subject == null || action == null || resource == null)
            {
                throw new ArgumentNullException();
            }

            Dictionary<object, ArrayList> subjects;

            if (this.resourceToSubjectsMap.TryGetValue(resource, out subjects))
            {
                ArrayList actions;

                if (subjects.TryGetValue(subject, out actions))
                {
                    if (actions.Contains(action))
                    {
                        actions.Remove(action);

                        if (actions.Count == 0)
                        {
                            subjects.Remove(actions);

                            if (subjects.Count == 0)
                            {
                                this.resourceToSubjectsMap.Remove(subjects);
                            }
                        }

                        return;
                    }
                }
            }

            throw new EnterpriseSecurityException(
                EM.AccessControl_RemoveInvalidRule,
                EM.AccessControl_RemoveInvalidRule);
        }
    }
}
