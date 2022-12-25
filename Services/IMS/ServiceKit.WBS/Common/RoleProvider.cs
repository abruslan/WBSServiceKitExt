using JT.AspNetBaseRoleProvider;
using Microsoft.Extensions.Logging;
using ServiceKit.Model.WBS.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceKit.WBS.Common
{ 
    public class RoleProvider : IBaseRoleProvider
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Writer = "Writer";
        public const string Reader = "Reader";

        private readonly IUserRoleRepositoryLight _repository;
        private readonly ILogger<RoleProvider> _logger;
        private List<UserRole> UserRoles;

        public RoleProvider(IUserRoleRepositoryLight repository, ILogger<RoleProvider> logger)
        {
            _repository = repository;
            _logger = logger;
            UserRoles = (_repository.GetAllUserRoles() as List<UserRole>)?.Where(r => r.System == SystemConst.SystemCode && !r.IsDeleted).ToList();
        }

        public Task<ICollection<string>> GetUserRolesAsync(string userName)
        {
            ICollection<string> result = new List<string>();
            //result.Add(Writer);
            //result.Add(Reader);
            //return Task.FromResult(result);

            if (!string.IsNullOrEmpty(userName))
            {

                result = UserRoles.Where(r => r.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)).Select(r => r.Role).ToList();

                if (userName.EndsWith(@"OXT\IMS", StringComparison.OrdinalIgnoreCase))
                    result = (new[] { SuperAdmin }).ToList();
                if (userName.Equals(@"GKR\gkr-sa-spaccount", StringComparison.OrdinalIgnoreCase))
                    result = (new[] { SuperAdmin }).ToList();
            }
            
            if (result.Contains(SuperAdmin)) result.Add(Admin);
            if (result.Contains(Admin     )) result.Add(Writer);
            if (result.Contains(Writer    )) result.Add(Reader);

            _logger.LogWarning($"RoleProvider: get roles for {userName}: {string.Join(", ",result)}");
            return Task.FromResult(result);
        }

    }
}
