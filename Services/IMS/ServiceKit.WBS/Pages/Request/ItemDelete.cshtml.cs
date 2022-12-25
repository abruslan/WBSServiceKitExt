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
    public class ItemDeleteModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ItemDeleteModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_RequestItem WBS_RequestItem { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_RequestItem = await _context.WBS_RequestItems.FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_RequestItem == null)
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

            WBS_RequestItem = await _context.WBS_RequestItems.FindAsync(id);

            if (WBS_RequestItem != null)
            {
                //_context.WBS_RequestItems.Remove(WBS_RequestItem);
                WBS_RequestItem.IsDeleted = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Details", new { id = WBS_RequestItem.ParentId });
        }
    }
}
