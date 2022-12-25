using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Request
{
    public class RequestProjectItemEditModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public RequestProjectItemEditModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Наименование для систем 1С и СЭД")]
        public string ShortName { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.WBS_RequestProjectItems
                .Include(w => w.Request).FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }
            Id = item.Id;
            ShortName = item.ShortName;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            WBS_RequestProjectItem item = _context.WBS_RequestProjectItems.Where(r => r.Id == Id).FirstOrDefault();
            if (item == null)
                return NotFound();

            item.ShortName = ShortName;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = item.RequestId });
        }
    }
}
