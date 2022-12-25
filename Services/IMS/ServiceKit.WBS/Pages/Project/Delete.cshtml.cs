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
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DeleteModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_Project WBS_Project { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
                return NotFound();

            WBS_Project = await _context.WBS_Projects.FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_Project == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            WBS_Project = await _context.WBS_Projects.FindAsync(WBS_Project.Id);
            if (WBS_Project == null)
                return NotFound();

            WBS_Project.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
