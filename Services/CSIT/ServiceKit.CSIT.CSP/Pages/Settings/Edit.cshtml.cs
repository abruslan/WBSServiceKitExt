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

namespace ServiceKit.CSIT.CSP.Pages.Settings
{
    public class EditModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public EditModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Setting Setting { get; set; }

        [BindProperty]
        public string Signer { get; set; }

        public List<SelectListItem> Signers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Setting =  await FirstOrDefaultAsync();
            if (Setting.Signer == null)
                Signers = await _context.Signers.Where(r => !r.IsDeleted).Select(r => new SelectListItem(r.Name, r.Id.ToString())).ToListAsync();
            else
                Signers = await _context.Signers.Where(r => !r.IsDeleted).Select(r => new SelectListItem(r.Name, r.Id.ToString(), Setting.Signer.Id == r.Id)).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Signers = await _context.Signers.Select(r => new SelectListItem(r.Name, r.Id.ToString())).ToListAsync();
                return Page();
            }

            var _Setting = await FirstOrDefaultAsync();
            if (Guid.TryParse(Signer, out var guid))
            {
                 _Setting.Signer = _context.Signers.Find(guid);

            }
            _context.Entry(_Setting).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("/CSP/Index");
        }

        private async Task<Setting> FirstOrDefaultAsync()
        {
            var _Setting = await _context.Settings.Include(r => r.Signer).FirstOrDefaultAsync();
            if (_Setting == null)
            {
                _Setting = new Setting();
                _context.Settings.Add(_Setting);
                await _context.SaveChangesAsync();
            }
            return _Setting;
        }
    }
}
