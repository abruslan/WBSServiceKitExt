using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.CSIT.CSP.Common
{
    public static class BuiltinRoles
    {
        /// <summary>
        /// Администратор – полный доступ к функциям системы.
        /// </summary>
        public const string Administrator = "Administrator";

        /// <summary>
        /// CSP - доступны функции обработки CSP
        /// </summary>
        public const string RegisterCSP = "CSP";

        public static string[] Roles
            => new string[] { Administrator, RegisterCSP };
    }
}
