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

namespace Owasp.Esapi.Interfaces
{
    /// <summary>
    ///     The IAccessController interface defines a set of methods that can be used in a wide variety of applications to
    ///     enforce access control. In most applications, access control must be performed in multiple different locations
    ///     across the various application layers.
    ///     The interface is built around the idea of subjects, actions, and resources. A specific subject/action/resource
    ///     combination is known as a rule.
    /// </summary>
    public interface IAccessController
    {
        /// <summary>
        ///     Determines whether the currently authenticated subject is authorized to perform a particular action to a
        ///     particular resource.
        /// </summary>
        /// <param name="action">The action to perform.</param>
        /// <param name="resource">The resource to perform the action on.</param>
        /// <returns>True, if access is allowed. False, otherwise.</returns>
        bool IsAuthorized(object action, object resource);

        /// <summary>
        ///     Determines whether a given subject is authorized to perform a particular action to a particular resource.
        /// </summary>
        /// <param name="subject">The subject who is performing the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="resource">The resource to perform the action on.</param>
        /// <returns>True, if access is allowed. False, otherwise.</returns>
        bool IsAuthorized(object subject, object action, object resource);

        /// <summary>
        ///     Adds an access control rule, defined as a subject, an action, and a resource.
        /// </summary>
        /// <param name="subject">The subject who is performing the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="resource">The resource to perform the action on.</param>
        void AddRule(object subject, object action, object resource);

        /// <summary>
        ///     Removes an access control rule, defined as a subject, an action, and a resource. Throws an exception if rule
        /// </summary>
        /// <param name="subject">The subject who is performing the action.</param>
        /// <param name="action">The action to perform.</param>
        /// <param name="resource">The resource to perform the action on.</param>
        void RemoveRule(object subject, object action, object resource);
    }
}
