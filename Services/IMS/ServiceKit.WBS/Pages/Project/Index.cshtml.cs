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
    public class IndexModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public IndexModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_Project> WBS_Project { get;set; }

        public async Task OnGetAsync()
        {
            WBS_Project = await _context.WBS_Projects.Where(r => !r.IsDeleted).ToListAsync();
        }
    }
}
