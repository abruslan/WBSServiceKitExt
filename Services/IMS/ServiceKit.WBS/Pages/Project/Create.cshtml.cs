using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
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
        public WBS_Project WBS_Project { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(WBS_Project.ProjectCode))
            { 
                ModelState.AddModelError("WBS_Project.ProjectCode", "Необходимо указать код проекта");
                return Page();
            }
            if (_context.WBS_Projects.Any(r => r.ProjectCode == WBS_Project.ProjectCode && !r.IsDeleted))
                ModelState.AddModelError("WBS_Project.ProjectCode", "Проект с таким кодом уже существует");

            if (!ModelState.IsValid)
                return Page();

            _context.WBS_Projects.Add(WBS_Project);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
