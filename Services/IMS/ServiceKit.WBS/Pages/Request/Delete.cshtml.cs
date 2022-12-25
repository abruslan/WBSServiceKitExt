using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DeleteModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_Request WBS_Request { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_Request = await _context.WBS_Requests.FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_Request == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_Request = await _context.WBS_Requests.FindAsync(id);

            if (WBS_Request != null)
            {
                WBS_Request.IsDeleted = true;
                //_context.WBS_Requests.Remove(WBS_Request);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
