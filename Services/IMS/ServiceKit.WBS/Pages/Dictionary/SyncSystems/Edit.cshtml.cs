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

namespace ServiceKit.WBS.Pages.Dictionary.SyncSystems
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Admin)]
    public class EditModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public EditModel(ServiceKit.Model.WBS.AppDbContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(WBS_SyncSystem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WBS_SyncSystemExists(WBS_SyncSystem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool WBS_SyncSystemExists(Guid id)
        {
            return _context.WBS_SyncSystems.Any(e => e.Id == id);
        }
    }
}
