using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;

namespace ServiceKit.WBS.Pages.Project
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class EditModel : PageModel
    {
        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public EditModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_Project WBS_Project { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_Project = await _context.WBS_Projects.FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_Project == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(WBS_Project.ProjectCode))
            {
                ModelState.AddModelError("WBS_Project.ProjectCode", "Необходимо указать код проекта");
                return Page();
            }
            if (_context.WBS_Projects.Any(r => r.ProjectCode == WBS_Project.ProjectCode && !r.IsDeleted && r.Id != WBS_Project.Id))
                ModelState.AddModelError("WBS_Project.ProjectCode", "Проект с таким кодом уже существует");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var project = await _context.WBS_Projects.FirstOrDefaultAsync(r => r.Id == WBS_Project.Id);
            if (project == null)
                return NotFound();

            project.ProjectCode = WBS_Project.ProjectCode;
            project.ProjectName = WBS_Project.ProjectName;
            project.ProjectShortName = WBS_Project.ProjectShortName;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private bool WBS_ProjectExists(Guid id)
        {
            return _context.WBS_Projects.Any(e => e.Id == id);
        }
    }
}
