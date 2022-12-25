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
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DeleteModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserRole UserRole { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserRole = await _context.UserRoles.FirstOrDefaultAsync(m => m.Id == id);

            if (UserRole == null)
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

            //UserRole = await _context.UserRoles.FindAsync(id);
            UserRole = await _context.UserRoles.FirstOrDefaultAsync(m => m.Id == id && m.System == Common.SystemConst.SystemCode);

            if (UserRole != null)
            {
                UserRole.IsDeleted = true;
                //_context.UserRoles.Remove(UserRole);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
