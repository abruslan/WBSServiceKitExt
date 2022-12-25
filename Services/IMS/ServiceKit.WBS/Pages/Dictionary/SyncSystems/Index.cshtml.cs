using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Dictionary.SyncSystems
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Admin)]
    public class IndexModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public IndexModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_SyncSystem> WBS_SyncSystem { get;set; }

        public async Task OnGetAsync()
        {
            WBS_SyncSystem = await _context.WBS_SyncSystems.ToListAsync();
        }
    }
}
