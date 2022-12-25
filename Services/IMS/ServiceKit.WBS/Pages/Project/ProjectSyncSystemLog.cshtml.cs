using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.Model.WBS.PayDox;
using ServiceKit.WBS.Processors.PayDox;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Reader)]
    public class ProjectSyncSystemLogModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ProjectSyncSystemLogModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_SyncRequestItem> WBS_SyncRequestItems { get;set; }
        public WBS_SyncSystem WBS_SyncSystem { get;set; }
        public WBS_Project WBS_Project { get;set; }

        public async Task<IActionResult> OnGetAsync(Guid id, Guid system, Guid request)
        {
            WBS_SyncSystem = await _context.WBS_SyncSystems.Where(r => r.Id == system).FirstOrDefaultAsync();
            WBS_Project = await _context.WBS_Projects.Where(r => r.Id == id).FirstOrDefaultAsync();
            if (WBS_SyncSystem == null || WBS_Project == null)
                return NotFound();

            var provider = new ServiceKit.PayDox.PayDoxProvider(new PayDoxConfiguration(WBS_SyncSystem));
            WBS_SyncRequestItems = provider.ReadSyncRequestItems(WBS_Project, request);
            return Page();
        }
    }
}
