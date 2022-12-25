using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public DeleteModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Register Register { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Register = await _context.Registers.FirstOrDefaultAsync(m => m.Id == id);

            if (Register == null)
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

            Register = await _context.Registers.Where(r => r.Id == id).Include(r => r.Items).FirstOrDefaultAsync();

            if (Register != null)
            {
                _context.RemoveRange(Register.Items);
                _context.Registers.Remove(Register);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
