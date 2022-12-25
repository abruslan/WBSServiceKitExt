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
using ServiceKit.CSIT.CSP.Processors;
using ServiceKit.Helpers.Common;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    [Authorize]
    public class ChangeStatusModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ChangeStatusModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string id, RegisterStatus status)
        {
            var guid = Guid.Parse(id);
            var register = _context.Registers.Where(r => r.Id == guid)
                .Include(r => r.Items)
                .ThenInclude(r => r.Client)
                .Include(r => r.Items)
                .ThenInclude(r => r.ClientService)
                .FirstOrDefault();
            if (register == null)
                return NotFound();

            var generator = new ClientGenerator(_context);
            switch (status)
            {
                case RegisterStatus.None:
                case RegisterStatus.Imported:
                    // Clear reports
                    break;
                case RegisterStatus.Checking:
                    generator.GenerateClient(register);
                    break;
                case RegisterStatus.Checked:
                    generator.GenerateReport(register);
                    break;
            }

            register.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnGetGenerateTo1CAsync(string id)
        {
            var guid = Guid.Parse(id);
            var register = _context.Registers.Where(r => r.Id == guid)
                .Include(r => r.Items)
                .ThenInclude(r => r.Client)
                .Include(r => r.Items)
                .ThenInclude(r => r.ClientService)
                .ThenInclude(r => r.ClientAnnex)
                .FirstOrDefault();
            if (register == null)
                return NotFound();
            var data = new ZipFor1CGenerator(_context).Zip(register);
            return File(data, "application/zip", $"{register.Account}.zip".RemoveInvalidFileNameChars());
        }
    }
}
