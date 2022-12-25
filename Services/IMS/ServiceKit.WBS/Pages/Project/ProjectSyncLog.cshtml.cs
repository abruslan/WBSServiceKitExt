using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Reader)]
    public class ProjectSyncLogModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ProjectSyncLogModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_SyncLogItem> WBS_SyncLogItems { get;set; }
        public WBS_SyncLog WBS_SyncLog { get;set; }
        public WBS_Project WBS_Project { get;set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {

            WBS_SyncLog = await _context.WBS_SyncLogs.Include(r => r.Project).Where(r => r.Id == id).FirstOrDefaultAsync();
            if (WBS_SyncLog == null)
                return NotFound();

            WBS_Project = WBS_SyncLog.Project;

            WBS_SyncLogItems = await _context.WBS_SyncLogItems
                .Include(w => w.SyncLog).Where(r => r.WBS_SyncLogId == id).ToListAsync();

            return Page();
        }
    }
}
