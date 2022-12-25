using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class EditModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public EditModel(ServiceKit.Model.WBS.AppDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var request = _context.WBS_Requests.FirstOrDefault(r => r.Id == WBS_Request.Id);
            if (request == null)
                return NotFound();

            request.ProjectCode = WBS_Request.ProjectCode;
            request.ProjectName = WBS_Request.ProjectName;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id = WBS_Request.Id });
        }

        private bool WBS_RequestExists(Guid id)
        {
            return _context.WBS_Requests.Any(e => e.Id == id);
        }
    }
}
