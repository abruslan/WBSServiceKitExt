using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class ItemCreateModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ItemCreateModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid ParentId { get; set; }

        public IActionResult OnGet(Guid parentid)
        {
            return Page();
        }

        [BindProperty]
        public WBS_RequestItem WBS_RequestItem { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(Guid parentid)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            WBS_RequestItem.ParentId = parentid;
            _context.WBS_RequestItems.Add(WBS_RequestItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = WBS_RequestItem.ParentId });
        }
    }
}
