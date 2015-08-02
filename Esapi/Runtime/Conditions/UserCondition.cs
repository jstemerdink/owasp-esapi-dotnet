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
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Owasp.Esapi.Runtime.Conditions
{
    /// <summary>
    ///     User test condition
    /// </summary>
    public class UserCondition : ICondition
    {
        /// <summary>
        ///     Any user name pattern
        /// </summary>
        public const string AnyNamePattern = "*";

        private List<string> _roles;

        private Regex _userName;

        /// <summary>
        ///     Initialize user test condition
        /// </summary>
        /// <param name="namePattern">User name pattern</param>
        public UserCondition(string namePattern)
            : this(namePattern, null)
        {
        }

        /// <summary>
        ///     Initialize user test condition
        /// </summary>
        /// <param name="namePattern">User name pattern</param>
        /// <param name="roles">User roles</param>
        public UserCondition(string namePattern, IEnumerable<string> roles)
        {
            this.NamePattern = namePattern;
            this.Roles = roles;
        }

        /// <summary>
        ///     Name pattern
        /// </summary>
        public string NamePattern
        {
            get
            {
                return this._userName.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._userName = new Regex("^$");
                }
                else
                {
                    this._userName = new Regex(value);
                }
            }
        }

        /// <summary>
        ///     User roles
        /// </summary>
        public IEnumerable<string> Roles
        {
            get
            {
                return this._roles;
            }
            set
            {
                this._roles = (value == null ? new List<string>() : new List<string>(value));
            }
        }

        #region ICondition Members

        /// <summary>
        ///     Test if the current user identity and roles matches
        /// </summary>
        /// <returns></returns>
        public bool Evaluate(ConditionArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // Get user identity 
            IPrincipal userPrincipal = Esapi.SecurityConfiguration.CurrentUser;
            IIdentity userIdentity = (userPrincipal != null ? userPrincipal.Identity : null);

            if (userIdentity == null)
            {
                return false;
            }

            // Get user name
            string userName = userIdentity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return false;
            }

            // Match user name
            if (!this._userName.IsMatch(userName))
            {
                return false;
            }

            // Match roles
            foreach (string role in this._roles)
            {
                if (!userPrincipal.IsInRole(role))
                {
                    return false;
                }
            }

            // Roles match
            return true;
        }

        #endregion
    }
}
