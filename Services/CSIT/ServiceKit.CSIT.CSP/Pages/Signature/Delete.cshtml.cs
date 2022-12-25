using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.Signature
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public DeleteModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Signer Signer { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Signer = await _context.Signers.FirstOrDefaultAsync(m => m.Id == id);

            if (Signer == null)
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

            Signer = await _context.Signers.FindAsync(id);

            if (Signer != null)
            {
                _context.Signers.Remove(Signer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/CSP/Index");
        }
    }
}
