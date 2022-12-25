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
    public class DetailsModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DetailsModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public UserRole UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //UserRole = await _context.UserRoles.FirstOrDefaultAsync(m => m.Id == id);
            UserRole = await _context.UserRoles.FirstOrDefaultAsync(m => m.Id == id && m.System == Common.SystemConst.SystemCode);

            if (UserRole == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
