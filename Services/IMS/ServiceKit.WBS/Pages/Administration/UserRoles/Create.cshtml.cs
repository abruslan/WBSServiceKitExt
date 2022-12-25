using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Administration.UserRoles
{
    public class CreateModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public CreateModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserRole UserRole { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserRole.System = Common.SystemConst.SystemCode;
            _context.UserRoles.Add(UserRole);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
