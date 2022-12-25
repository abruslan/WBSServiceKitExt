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
    public class ProjectPublishedModel : PageModel
    {
        public class SyncSystem
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public bool Selected { get; set; }
        }

        private readonly ServiceKit.Model.WBS.AppDbContext _context;

        public ProjectPublishedModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_Project WBS_Project { get; set; }
        public List<WBS_ProjectPublication> PublicationList { get; set; }

        [BindProperty]
        public List<SyncSystem> SyncSystems { get; set; }

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

            SyncSystems = await _context.WBS_SyncSystems
                .Where(r => !r.IsDeleted)
                .Where(r => r.SystemType == WBS_SyncSystemType.Export1C || r.SystemType == WBS_SyncSystemType.ExportPayDox)
                .Select(r => new SyncSystem() { Id = r.Id, Name = r.Name, Selected = false })
                .ToListAsync();
            
            PublicationList = await _context.WBS_ProjectPublications
                .Include(r => r.SyncSystem)
                .Where(r => r.ProjectId == id && !r.SyncSystem.IsDeleted).ToListAsync();

            PublicationList.ForEach(l => {
                SyncSystems.Where(s => s.Id == l.SyncSystemId).First().Selected = true;
            });

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

            Guid id = WBS_Project.Id;
            PublicationList = await _context.WBS_ProjectPublications.Where(r => r.ProjectId == id).ToListAsync();
            _context.WBS_ProjectPublications.RemoveRange(PublicationList);

            var newlist = SyncSystems.Where(r => r.Selected).Select(r => new WBS_ProjectPublication() { ProjectId = id, SyncSystemId = r.Id }).ToList();
            _context.WBS_ProjectPublications.AddRange(newlist);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Card", new { id });
        }

        private bool WBS_ProjectExists(Guid id)
        {
            return _context.WBS_Projects.Any(e => e.Id == id);
        }
    }
}
