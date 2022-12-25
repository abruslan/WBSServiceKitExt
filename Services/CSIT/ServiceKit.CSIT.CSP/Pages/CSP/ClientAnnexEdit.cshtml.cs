using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    public class ClientAnnexEditModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public ClientAnnexEditModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ClientAnnex ClientAnnex { get; set; }
        [BindProperty]
        public Guid registerId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id, Guid register)
        {
            if (id == null)
            {
                return NotFound();
            }

            registerId = register;
            ClientAnnex = await _context.ClientAnnexes.FirstOrDefaultAsync(m => m.Id == id);

            if (ClientAnnex == null)
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

            _context.Attach(ClientAnnex).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientAnnexExists(ClientAnnex.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = registerId });
        }

        private bool ClientAnnexExists(Guid id)
        {
            return _context.ClientAnnexes.Any(e => e.Id == id);
        }
    }
}
