using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Common
{
    public interface IUserRoleRepositoryLight
    {
        public IEnumerable GetAllUserRoles();

    }
}
