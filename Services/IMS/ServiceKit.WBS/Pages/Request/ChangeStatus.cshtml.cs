using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServiceKit.Model.WBS;
using ServiceKit.Model.WBS.Entry;
using ServiceKit.WBS.Processors.Request;

namespace ServiceKit.WBS.Pages.CSP
{
    [Authorize]
    public class ChangeStatusModel : PageModel
    {
        private readonly AppDbContext _context;

        public ChangeStatusModel(AppDbContext context)
        {
            _context = context;
        }

        [Obsolete]
        public async Task<IActionResult> OnGetAsync(string id, WBS_RequestStatus status)
        {
            var guid = Guid.Parse(id);
            var request = _context.WBS_Requests.Where(r => r.Id == guid)
                .Include(r => r.Changes)
                .Include(r => r.ProjectItems)
                .FirstOrDefault();
            if (request == null)
                return NotFound();

            //switch (status)
            //{
            //    case WBS_RequestStatus.Checked:
            //        if (new ProjectRequestProcessor(_context).Check(request))
            //        {
            //            return RedirectToPage("./Details", new { id });
            //        }
            //        break;
            //    case WBS_RequestStatus.Completed:
            //        var project = new ProjectRequestProcessor(_context).Eval(request); ;
            //        if (project != null)
            //            return RedirectToPage("/Project/Item", new { id = project.Id });
            //        break;
            //}

            request.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToPage("./Details", new { id });
        }
    }
}
