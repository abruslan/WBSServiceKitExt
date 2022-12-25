using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Common
{
    public class UserRoleRepositoryLight : IUserRoleRepositoryLight
    {
        private readonly IEnumerable _userRoles;
        public UserRoleRepositoryLight(IEnumerable userRoles)
        {
            _userRoles = userRoles;
        }
        public IEnumerable GetAllUserRoles()
        {
            return _userRoles;
        }
    }
}
