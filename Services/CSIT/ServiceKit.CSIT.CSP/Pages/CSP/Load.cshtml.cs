using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceKit.CSIT.CSP.Data;
using ServiceKit.CSIT.CSP.Data.Entry;
using ServiceKit.CSIT.CSP.Processors;

namespace ServiceKit.CSIT.CSP.Pages.CSP
{
    [Authorize]
    public class LoadModel : PageModel
    {
        private readonly ServiceKit.CSIT.CSP.Data.ApplicationDbContext _context;

        public LoadModel(ServiceKit.CSIT.CSP.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public IFormFile Upload { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile Upload)
        {
            if (Upload == null || Upload.Length <= 0)
                return BadRequest("File is empty");

            if (!Path.GetExtension(Upload.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                return BadRequest("File extension is not supported");

            if (!ModelState.IsValid)
            {
                return Page();
            }



            var processor = new ExcelCSPProcessor(_context);
            var register = processor.Eval(Upload);
            if (register == null) 
            {
                return BadRequest(processor.ErrorMessage);
                //return Page();
            }


            _context.Registers.Add(register);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
