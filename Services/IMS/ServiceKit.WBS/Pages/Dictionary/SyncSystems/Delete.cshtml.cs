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
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DeleteModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_SyncSystem WBS_SyncSystem { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_SyncSystem = await _context.WBS_SyncSystems.FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_SyncSystem == null)
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

            WBS_SyncSystem = await _context.WBS_SyncSystems.FindAsync(id);

            if (WBS_SyncSystem != null)
            {
                _context.WBS_SyncSystems.Remove(WBS_SyncSystem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
