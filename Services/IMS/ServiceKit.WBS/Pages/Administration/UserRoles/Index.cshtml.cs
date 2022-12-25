using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Administration.UserRoles
{
    public class IndexModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public IndexModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<UserRole> UserRole { get;set; }

        public async Task OnGetAsync()
        {
            UserRole = await _context.UserRoles.Where(r => !r.IsDeleted && r.System == "WBS").ToListAsync();
        }
    }
}
