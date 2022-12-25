using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Common;
using ServiceKit.WBS.Processors.Request;

namespace ServiceKit.WBS.Pages.Request
{
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = Common.RoleProvider.Writer)]
    public class DetailsModel : PageModel
    {
        public readonly ServiceKit.Model.WBS.AppDbContext _context;

        public DetailsModel(ServiceKit.Model.WBS.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WBS_Request WBS_Request { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WBS_Request = await _context.WBS_Requests
                .Include(r => r.Changes)
                .Include(r => r.ProjectItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (WBS_Request == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCheckAsync()
        {
            var id = WBS_Request.Id;
            WBS_Request = await _context.WBS_Requests
                .Include(r => r.Changes)
                .Include(r => r.ProjectItems)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (WBS_Request == null)
                return NotFound();


            var processor = WBS_Request.CreateRequestProcessor(_context);
            if (!processor.Check(WBS_Request))
            {
                foreach(var err in processor.ErrorMessage)
                    ModelState.AddModelError("", err);
                await _context.SaveChangesAsync();
                return Page();
            }

            WBS_Request.Status = WBS_RequestStatus.Checked;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id });
        }

        public async Task<IActionResult> OnPostEvalRequestAsync()
        {
            var id = WBS_Request.Id;
            WBS_Request = await _context.WBS_Requests
                .Include(r => r.Changes)
                .Include(r => r.ProjectItems)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (WBS_Request == null)
                return NotFound();

            var WBS_Project = await _context.WBS_Projects.FirstOrDefaultAsync(r => r.ProjectCode == WBS_Request.ProjectCode && !r.IsDeleted);
            if (WBS_Project == null)
            {
                ModelState.AddModelError("", $"Не найден проект с кодом {WBS_Request.ProjectCode}. До загрузки заявки необходимо создать проект.");
                return Page();
            }

            var processor = WBS_Request.CreateRequestProcessor(_context);
            var project = processor.Eval(WBS_Request);
            if (project != null)
            {
                WBS_Request.Status = WBS_RequestStatus.Completed;
                await _context.SaveChangesAsync();
                return RedirectToPage("/Project/Item", new { id = project.Id });
            }

            foreach(var err in processor.ErrorMessage)
                ModelState.AddModelError("", err);
            return Page();
        }
    }
}
