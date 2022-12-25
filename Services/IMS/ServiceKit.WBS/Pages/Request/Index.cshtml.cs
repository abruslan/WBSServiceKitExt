using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Common;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class IndexModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public IndexModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IList<WBS_Request> WBS_Request { get;set; }

        public async Task OnGetAsync()
        {
            WBS_Request = await _context.WBS_Requests.Where(r => !r.IsDeleted).ToListAsync();
        }
    }
}
